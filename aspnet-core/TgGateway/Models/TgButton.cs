namespace TgDrive.Domain.Telegram.Models;

public record TgButton(string Text, string Callback, IEnumerable<string> Args);
