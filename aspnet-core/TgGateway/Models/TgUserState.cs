namespace TgGateway.Models;

public record TgUserState(
    long UserId,
    long ChatId,
    Dictionary<string, string> Values
);
