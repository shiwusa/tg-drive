using LiteDB;

namespace TgDrive.CacheStorage.LiteDB.Models;

public class UserDocument
{
    [BsonId] public string Id => $"{ChatId}{UserId}";

    public long ChatId { get; set; }
    public long UserId { get; set; }
    public Dictionary<string, string> State { get; set; }
}
