using TgDrive.Domain.Telegram.Models;

namespace TgDrive.BotClient.Frontend.Helpers;

public static class KeyboardBuilder
{
    public static TgKeyboard FromColumn(
        string menuCallbackId,
        TgButton[] buttons)
    {
        var keys = new List<List<TgButton>>();
        foreach (var btn in buttons)
        {
            var inlineBtn = new TgButton(btn.Text,
                $"{menuCallbackId} {btn.Callback} {string.Join(' ', btn.Args)}",
                btn.Args);
            keys.Add(new List<TgButton> {inlineBtn});
        }

        var keyboard = new TgKeyboard(keys);
        return keyboard;
    }
}
