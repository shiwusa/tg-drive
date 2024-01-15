﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgGateway.Abstractions;
using TgGateway.Models;
using TgGateway.Models.Updates;
using IUpdateHandler = Telegram.Bot.Polling.IUpdateHandler;

namespace TgGateway.Implementations;

public class UpdateParser : IUpdateHandler
{
    private readonly IMessageStorage _storage;
    private readonly Abstractions.IUpdateHandler _updateHandler;

    public UpdateParser(
        IMessageStorage storage,
        Abstractions.IUpdateHandler updateHandler)
    {
        _storage = storage;
        _updateHandler = updateHandler;
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    await ProcessMessage(update.Message!);
                    break;
                case UpdateType.CallbackQuery:
                    await ProcessCallbackQuery(update.CallbackQuery!);
                    break;
                default:
                    return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        await ProcessError(exception);
    }

    private async Task ProcessError(Exception error)
    {
        await _updateHandler.HandleError(error);
    }

    private async Task ProcessCallbackQuery(CallbackQuery callback)
    {
        if (string.IsNullOrEmpty(callback.Data))
        {
            return;
        }

        var parts = callback.Data.Split();
        string? menuId, btnId;
        List<string> args;
        try
        {
            menuId = parts[0];
            btnId = parts[1];
            args = parts.Skip(2)
                .ToList();
        }
        catch (Exception)
        {
            return;
        }

        var userState =
            await _storage.GetUserState(callback.Message!.Chat.Id, callback.From.Id);

        await _updateHandler.HandleCallback(new TgCallbackUpdate(
            DateTime: DateTime.Now,
            MenuId: menuId,
            ButtonId: btnId,
            Arguments: args,
            ChatId: callback.Message!.Chat.Id,
            SenderId: callback.From.Id,
            State: userState.values
        ));
    }

    private async Task ProcessMessage(Message msg)
    {
        if (msg.From == null)
        {
            return;
        }

        string msgText = msg.Caption ?? msg.Text ?? msg.Document?.FileName ?? msg.Audio?.Title;
        await _storage.SaveMessage(new TgMessage(
            ChatId: msg.Chat.Id,
            DateTime: msg.Date,
            MessageId: msg.MessageId,
            Purpose: TgMessagePurpose.Message,
            SenderId: msg.From!.Id,
            Type: (TgMessageType)msg.Type,
            Text: msgText
        ));
        var userState =
            await _storage.GetUserState(msg.Chat.Id, msg.From.Id);
        if (msg.Text?.StartsWith("/") == true)
        {
            var command = new string(msg.Text.Skip(1)
                .ToArray());
            if (command == string.Empty)
            {
                return;
            }

            await _updateHandler.HandleCommand(new TgCommandUpdate(
                ChatId: msg.Chat.Id,
                DateTime: msg.Date,
                Command: command,
                SenderId: msg.From.Id,
                State: userState.values
            ));
        }
        else
        {
            await _updateHandler.HandleMessage(new TgMessageUpdate(
                ChatId: msg.Chat.Id,
                SenderId: msg.From.Id,
                DateTime: msg.Date,
                State: userState.values,
                Message: new TgMessage(
                    ChatId: msg.Chat.Id,
                    MessageId: msg.MessageId,
                    DateTime: msg.Date,
                    SenderId: msg.From!.Id,
                    Type: (TgMessageType)msg.Type,
                    Purpose: TgMessagePurpose.Unknown,
                    Text: msgText,
                    ForwardedFromChatId: msg.ForwardFromChat?.Id ?? 0
                )
            ));
        }
    }
}
