namespace TgDrive.Domain.Telegram.Updates;

public abstract record TgUpdate(
    long SenderId,
    long ChatId,
    DateTime DateTime,
    Dictionary<string, string> State
);
