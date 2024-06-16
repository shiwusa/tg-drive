using TgDrive.Domain.Shared;
using TgDrive.Domain.Services;
using TgDrive.BotClient.Frontend.Abstractions;
using TgDrive.BotClient.Frontend.Models;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Models;

namespace TgDrive.BotClient.Frontend.Menus;

[TgMenu("root")]
public class RootMenu : MenuBase
{
    private readonly IDirectoryService _directoryService;
    private readonly IRedirectHandler _redirectHandler;

    public RootMenu(
        ITgDriveBotClient botClient,
        IDirectoryService directoryService,
        IRedirectHandler redirectHandler)
        : base(botClient)
    {
        _directoryService = directoryService;
        _redirectHandler = redirectHandler;
    }


    [TgButtonCallback("as")]
    public async Task MenuBtn_AddSubdir(long chatId, IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send a name for a new directory:");
    }

    [TgMessageResponse("as")]
    public async Task AddSubdir(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.Text == null)
        {
            await _botClient.SendText(
                chatId,
                "Please, send text message with a name for a new directory");
            return;
        }

        var newDirectory = new DirectoryDto
        {
            Name = message.Text, OwnerId = (long)message.SenderId!, ParentId = null
        };
        await _directoryService.AddDirectory(chatId, newDirectory);
        await Open(chatId);
    }

    [TgButtonCallback("os")]
    public async Task MenuBtn_OpenSubdir(long chatId, IEnumerable<string> parameters)
    {
        var directoryId = long.Parse(parameters.First());
        await _redirectHandler.Redirect(chatId, typeof(DirectoryMenu), directoryId);
    }

    private TgKeyboard GetKeyboard(IEnumerable<DirectoryDto> subdirs)
    {
        var buttons = new List<TgMenuButton>();
        foreach (var subdir in subdirs)
        {
            buttons.Add(new TgMenuButton(subdir.Name, MenuBtn_OpenSubdir, subdir.Id));
        }
        buttons.AddRange(new List<TgMenuButton> {
            new("Add directory", MenuBtn_AddSubdir)
        });
        var keyboard = GetKeyboard(buttons);
        return keyboard;
    }

    public async Task Open(long chatId)
    {
        var dirs = await _directoryService.GetRoot(chatId);
        var keyboard = GetKeyboard(dirs);
        var menu = new MenuData("You are in the root of your storage", keyboard);
        await _botClient.SendMenu(chatId, menu);
    }
}
