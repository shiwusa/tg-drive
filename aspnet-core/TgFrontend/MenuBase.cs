﻿using System.Reflection;
using TgDrive.BotClient.Frontend.Helpers;
using TgDrive.BotClient.Frontend.Models;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Models;
using TgDrive.Domain.Telegram.Updates;

namespace TgDrive.BotClient.Frontend;

public abstract class MenuBase
{
    protected readonly ITgDriveBotClient _botClient;

    protected MenuBase(ITgDriveBotClient botClient)
    {
        _botClient = botClient;
    }

    protected string GetSelfId()
    {
        var type = this.GetType();
        var attribute = type.GetCustomAttribute<TgMenuAttribute>(false);
        if (attribute == null)
        {
            throw new ApplicationException("Menu handler should specify TgMenuAttribute");
        }

        return attribute.MenuId;
    }

    protected string GetBtnId(ButtonCallbackHandler handler)
    {
        var methodInfo = handler.Method;
        var attribute = methodInfo.GetCustomAttribute<TgButtonCallbackAttribute>(false);
        if (attribute == null)
        {
            throw new ArgumentException("Passed method was of wrong signature!");
        }

        return attribute.ButtonId;
    }

    protected MessageResponseHandler? GetMessageResponseHandler(string btnId)
    {
        var methods = GetType().GetMethods();
        var callbackHandlers = methods.Select(m => new
            {
                msgResponseAttribute = m
                    .GetCustomAttributes(typeof(TgMessageResponseAttribute), false)
                    .Cast<TgMessageResponseAttribute>()
                    .FirstOrDefault(),
                handlerFn = m
            })
            .Where(x => x.msgResponseAttribute != null);
        
        var matchingHandler = callbackHandlers
            .FirstOrDefault(h => h.msgResponseAttribute!.ButtonId == btnId);
        if (matchingHandler == null)
        {
            return null;
        }

        var resultDelegate = (MessageResponseHandler)Delegate.CreateDelegate(
            typeof(MessageResponseHandler),
            this,
            matchingHandler.handlerFn.Name);
        return resultDelegate;
    }

    protected ButtonCallbackHandler? GetButtonCallbackHandler(string btnId)
    {
        var methods = GetType().GetMethods();
        var callbackHandlers = methods.Select(m => new
            {
                btnCallbackAttribute = m
                    .GetCustomAttributes(typeof(TgButtonCallbackAttribute), false)
                    .Cast<TgButtonCallbackAttribute>()
                    .FirstOrDefault(),
                menuMethod = m
            })
            .Where(x => x.btnCallbackAttribute != null);
        
        var matchingHandler = callbackHandlers.FirstOrDefault(
            h => h.btnCallbackAttribute!.ButtonId == btnId);
        if (matchingHandler == null)
        {
            return null;
        }

        var resultDelegate = (ButtonCallbackHandler)Delegate.CreateDelegate(
            typeof(ButtonCallbackHandler),
            this,
            matchingHandler.menuMethod.Name);
        return resultDelegate;
    }

    protected TgKeyboard GetKeyboard(IEnumerable<TgMenuButton> menuButtons)
    {
        var buttons = menuButtons
            .Select(mb => new TgButton(mb.Text, GetBtnId(mb.Handler), mb.Args));
        var keyboard = KeyboardBuilder.FromColumn(GetSelfId(), buttons.ToArray());
        return keyboard;
    }

    public virtual bool FitsCallbackId(string menuId)
    {
        return GetSelfId() == menuId;
    }

    public virtual async Task ProcessCallback(TgCallbackUpdate update)
    {
        var handler = GetButtonCallbackHandler(update.ButtonId);
        if (handler == null)
        {
            return;
        }

        await _botClient.SetState(update.ChatId,
            update.SenderId,
            new Dictionary<string, string>
            {
                {"lastMenuId", update.MenuId},
                {"lastBtnId", update.ButtonId},
                {"lastArgs", update.Arguments.Aggregate("", (acc, str) => acc + str)}
            });
        await handler(update.ChatId, update.Arguments);
    }

    public async Task ProcessMessage(TgMessageUpdate update)
    {
        try
        {
            var lastBtnId = update.State["lastBtnId"];
            var lastArgs = update.State.GetValueOrDefault("lastArgs")
                ?.Split() ?? Array.Empty<string>();
            var handler = GetMessageResponseHandler(lastBtnId);
            if (handler == null)
            {
                return;
            }

            await handler(update.ChatId, lastArgs, update.Message);
        }
        catch (Exception ex)
        {
        }
    }
}
