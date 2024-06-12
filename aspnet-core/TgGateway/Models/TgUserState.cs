namespace TgDrive.Domain.Telegram.Models;

public record TgUserState(
    long UserId,
    long ChatId,
    Dictionary<string, string> Values
);
