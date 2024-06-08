using System;
using TgFrontend.Abstractions;
using TgGateway.Abstractions;

namespace TgFrontend;

public class RedirectHandler : IRedirectHandler
{
    private readonly IServiceProvider _serviceProvider;

    public RedirectHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Redirect(long chatId, Type menuType, params object[] args)
    {
        var menu = _serviceProvider.GetService(menuType);
        //var menu = _menus.FirstOrDefault(m => m.GetType() == menuType);
        if (menu == null)
        {
            return;
        }

        var openMethod = menuType.GetMethod("Open");
        if (openMethod == null)
        {
            return;
        }

        var result = openMethod.Invoke(menu, new object?[] { chatId }.Concat(args).ToArray());
        if (result?.GetType() != typeof(Task))
        {
            return;
        }

        await (Task)result;
    }
}
