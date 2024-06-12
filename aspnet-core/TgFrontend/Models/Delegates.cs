using TgDrive.Domain.Telegram.Models;

namespace TgDrive.BotClient.Frontend.Models;

public delegate Task ButtonCallbackHandler(long chatId, IEnumerable<string> parameters);

public delegate Task MessageResponseHandler(
    long chatId,
    IEnumerable<string> parameters,
    TgMessage message);
