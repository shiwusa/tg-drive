using TgGateway.Abstractions;

namespace TgFrontend.Abstractions;

public interface IRedirectHandler
{
    Task Redirect(long chatId, Type menuType, params object[] args);
}
