using TgDrive.BotClient.Frontend.Menus;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Updates;

namespace TgDrive.BotClient.Frontend;

public class TgDriveUpdateHandler : ITgDriveUpdateHandler
{
    private readonly RootMenu _rootMenu;
    private readonly SettingsMenu _settingsMenu;
    private readonly IEnumerable<MenuBase> _menus;

    public TgDriveUpdateHandler(
        RootMenu rootMenu,
        SettingsMenu settingsMenu,
        DirectoryMenu directoryMenu,
        FileMenu fileMenu)
    {
        _rootMenu = rootMenu;
        _settingsMenu = settingsMenu;
        _menus = new MenuBase[] { rootMenu, _settingsMenu, directoryMenu, fileMenu };
    }

    public async Task HandleCallback(TgCallbackUpdate update)
    {
        var menu = GetMenu(update.MenuId);
        if (menu == null)
        {
            return;
        }

        await menu.ProcessCallback(update);
    }

    public async Task HandleMessage(TgMessageUpdate update)
    {
        update.State.TryGetValue("lastMenuId", out var menuId);
        if (menuId == null)
        {
            return;
        }

        var menu = GetMenu(menuId);
        if (menu == null)
        {
            return;
        }

        await menu.ProcessMessage(update);
    }

    public async Task HandleCommand(TgCommandUpdate update)
    {
        switch (update.Command)
        {
            case "start":
                await _rootMenu.Open(update.ChatId);
                break;
            case "settings":
                await _settingsMenu.Open(update.ChatId);
                break;
            default: return;
        }
    }

    public Task HandleError(Exception error)
    {
        Console.WriteLine(error);
        return Task.CompletedTask;
    }

    private MenuBase? GetMenu(string menuId)
    {
        return _menus.FirstOrDefault(m => m.FitsCallbackId(menuId));
    }
}
