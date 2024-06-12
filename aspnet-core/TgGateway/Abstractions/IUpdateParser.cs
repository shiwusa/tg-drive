using ITelegramUpdateHandler = Telegram.Bot.Polling.IUpdateHandler;

namespace TgDrive.Domain.Telegram.Abstractions;

public interface IUpdateParser : ITelegramUpdateHandler
{
}
