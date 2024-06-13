using LiteDB;
using TgDrive.Domain.Telegram.Models;

namespace TgDrive.Infrastructure.LiteDB.Models;

public class ChatDocument
{
    [BsonId] public long ChatId { get; set; }

    public List<TgMessage> Messages { get; set; } = new();
}
