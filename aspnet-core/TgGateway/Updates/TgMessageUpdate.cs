using TgDrive.Domain.Telegram.Models;

namespace TgDrive.Domain.Telegram.Updates;

public record TgMessageUpdate(
        TgMessage Message,
        long SenderId,
        long ChatId,
        DateTime DateTime,
        Dictionary<string, string> State)
    : TgUpdate(SenderId, ChatId, DateTime, State);
