using TgDrive.Domain.Telegram.Models;

namespace TgDrive.Domain.Telegram.Abstractions;

public interface ITgDriveBotClient
{
    void StartReceiving(ITgDriveUpdateHandler handler);
    Task<long> SendMenu(long chatId, MenuData data);
    Task<long> SendTextUnmanaged(long chatId, string text);
    Task<long> SendText(long chatId, string text, long? replyToMsgId = null);
    Task<long> CopyMessage(
        long fromChat,
        long toChat,
        long messageId);
    Task<long> ForwardMessageUnmanaged(long fromChat, long toChat, long messageId);
    Task SetState(long chatId, long userId, Dictionary<string, string> state);

    IAsyncEnumerable<long> ForwardMessagesUnmanaged(
        long fromChat,
        long toChat,
        IEnumerable<long> messageIds);

    Task<long> CopyMessageUnmanaged(long fromChat, long toChat, long messageId);

    IAsyncEnumerable<long> CopyMessagesUnmanaged(
        long fromChat,
        long toChat,
        IEnumerable<long> messageIds);
}
