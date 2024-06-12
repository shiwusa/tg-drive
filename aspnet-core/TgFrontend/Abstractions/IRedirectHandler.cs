namespace TgDrive.BotClient.Frontend.Abstractions;

public interface IRedirectHandler
{
    Task Redirect(long chatId, Type menuType, params object[] args);
}
