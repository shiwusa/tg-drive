﻿using Microsoft.AspNetCore.Http;

namespace TgDrive.Web.Auth;

public class TgAuthMiddleware
{
    private readonly RequestDelegate _next;

    public TgAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.Value.StartsWith("/api"))
        {
            await _next.Invoke(context);
            return;
        }
        
        bool hashPresent = context.Request.Headers.TryGetValue(
            AuthorizationConsts.HashHeaderName, out var hashHeader);
        bool dataPresent = context.Request.Headers.TryGetValue(
            AuthorizationConsts.DataHeaderName, out var dataStringHeader);
        if (!hashPresent || !dataPresent)
        {
            await RejectAuth(context);
            return;
        }

        var hash = Uri.UnescapeDataString(hashHeader.First());
        var dataString = Uri.UnescapeDataString(dataStringHeader.First());

        bool dataStringValid = ValidateAuthData(dataString, hash);
        if (!dataStringValid)
        {
            await RejectAuth(context);
            return;
        }

        var parsed = ParseAuthData(dataString);
        context.Items[AuthorizationConsts.AuthDataItemName] = parsed;
        
        await _next.Invoke(context);
    }

    private bool ValidateAuthData(string dataString, string hash)
    {
        var tgBotToken = Environment.GetEnvironmentVariable("TGDRIVE_BOT_TOKEN");
        if (tgBotToken is null)
        {
            return false;
        }
        
        var signature = HashHelper.ComputeSha256HMACSignature(tgBotToken, dataString);
        return signature == hash;
    }
    
    private TelegramAuthData ParseAuthData(string dataString)
    {
        var pairs = dataString
            .Split('\n')
            .Select(x =>
            {
                var parts = x.Split('=');
                return (parts[0], parts[1]);
            })
            .ToDictionary(x => x.Item1, x => x.Item2);


        return new TelegramAuthData
        {
            Id = long.Parse(pairs["id"]),
            FirstName = pairs["first_name"],
            LastName = pairs.GetValueOrDefault("last_name") ?? "",
            Username = pairs["username"],
            PhotoUrl = pairs["photo_url"],
        };
    }

    private async Task RejectAuth(HttpContext context)
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("You should be authorized using Telegram widget.");
    }
}