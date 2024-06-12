namespace TgDrive.Domain.Telegram.Models;

public record TgKeyboard(IEnumerable<IEnumerable<TgButton>> Rows);
