using TgDrive.Domain.Services;
using TgDrive.BotClient.Frontend.Abstractions;
using TgDrive.BotClient.Frontend.Models;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Models;

namespace TgDrive.BotClient.Frontend.Menus;

[TgMenu("file")]
public class FileMenu : MenuBase
{
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly ITgFileService _tgFileService;
    private readonly IRedirectHandler _redirectHandler;

    public FileMenu(
        IDirectoryService directoryService,
        IFileService fileService,
        ITgFileService tgFileService,
        IRedirectHandler redirectHandler,
        ITgDriveBotClient botClient)
        : base(botClient)
    {
        _directoryService = directoryService;
        _fileService = fileService;
        _tgFileService = tgFileService;
        _redirectHandler = redirectHandler;
    }

    [TgButtonCallback("cd")]
    public async Task MenuBtn_ChangeDescription(
        long chatId,
        IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send new description for the file");
    }

    [TgMessageResponse("cd")]
    public async Task ChangeDescription(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.Text == null)
        {
            await _botClient.SendText(
                chatId,
                "Please, send text message with new directory description name");
            return;
        }

        var fileId = long.Parse(parameters.First());
        _ = await _fileService.ChangeDescription(chatId, fileId, message.Text);
        await Open(chatId, fileId);
        await _botClient.SendText(chatId, "Description changed successfully!");
    }

    [TgButtonCallback("cn")]
    public async Task MenuBtn_ChangeName(
        long chatId,
        IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send new name for the file");
    }

    [TgMessageResponse("cn")]
    public async Task ChangeName(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.Text == null)
        {
            await _botClient.SendText(
                chatId,
                "Please, send text message with new filename");
            return;
        }

        var fileId = long.Parse(parameters.First());
        _ = await _fileService.ChangeName(chatId, fileId, message.Text);
        await Open(chatId, fileId);
        await _botClient.SendText(chatId, "Name changed successfully!");
    }

    [TgButtonCallback("rem")]
    public async Task MenuBtn_Remove(long chatId, IEnumerable<string> parameters)
    {
        var fileId = long.Parse(parameters.First());
        var file = await _fileService.Remove(chatId, fileId);
        await _redirectHandler.Redirect(chatId, typeof(DirectoryMenu), file.DirectoryId);
        await _botClient.SendText(chatId, "Deleted successfully!");
    }

    [TgButtonCallback("back")]
    public async Task MenuBtn_GoBack(long chatId, IEnumerable<string> parameters)
    {
        var fileId = long.Parse(parameters.First());
        var file = await _fileService.GetFile(chatId, fileId);
        await _redirectHandler.Redirect(chatId, typeof(DirectoryMenu), file.DirectoryId);
    }

    private TgKeyboard GetKeyboard(long fileId)
    {
        var buttons = new List<TgMenuButton>
        {
            new("Remove", MenuBtn_Remove, fileId),
            new("Rename", MenuBtn_ChangeName, fileId),
            new("Change description", MenuBtn_ChangeDescription, fileId),
            new("Back", MenuBtn_GoBack, fileId)
        };
        var keyboard = GetKeyboard(buttons);
        return keyboard;
    }

    public async Task Open(long chatId, long fileId)
    {
        var file = await _fileService.GetFile(chatId, fileId);
        var keyboard = GetKeyboard(fileId);
        string title = $"{file.Name}\n" +
                       $"{file.Description}";
        await _botClient.SendMenu(chatId, new MenuData(title, keyboard));
        await _tgFileService.SendFile(chatId, fileId, chatId);
    }
}
