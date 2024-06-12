using TgDrive.Domain.Shared;
using TgDrive.Domain.Services;
using TgDrive.BotClient.Frontend.Abstractions;
using TgDrive.BotClient.Frontend.Models;
using TgDrive.Domain.Telegram.Abstractions;
using TgDrive.Domain.Telegram.Models;

namespace TgDrive.BotClient.Frontend.Menus;

[TgMenu("dir")]
public class DirectoryMenu : MenuBase
{
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly ITgFileService _tgFileService;
    private readonly IRedirectHandler _redirectHandler;

    public DirectoryMenu(
        ITgDriveBotClient botClient,
        IDirectoryService directoryService,
        IFileService fileService,
        ITgFileService tgFileService,
        IRedirectHandler redirectHandler)
        : base(botClient)
    {
        _directoryService = directoryService;
        _fileService = fileService;
        _tgFileService = tgFileService;
        _redirectHandler = redirectHandler;
    }

    [TgButtonCallback("af")]
    public async Task MenuBtn_AddFile(
        long chatId,
        IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send file to add");
    }

    [TgMessageResponse("af")]
    public async Task AddFile(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.Text == null)
        {
            await _botClient.SendText(
                chatId,
                "Message text should contain file name on the first line.");
            return;
        }

        var directoryId = long.Parse(parameters.First());
        var parts = message.Text.Split('\n');
        var name = parts.First();
        var descParts = parts.Skip(1);
        string? description = null;
        if (descParts.Any())
        {
            description = String.Join('\n', descParts);
        }

        var addedFile = await _tgFileService.AddFile(chatId, new FileDto
        {
            ChatId = chatId,
            AddedByUserId = chatId,
            Description = description,
            Name = name,
            DirectoryId = directoryId,
            MessageId = message.MessageId
        });
        if (addedFile == null)
        {
            await _botClient.SendText(chatId,
                "An error occured while adding file.\n" +
                "Please, make sure that the storage channel is configured and this bot is added " +
                "and has the rights to send messages there.\n\n" +
                "If you haven't set it up yet, just go to /settings and do it!.");
        }
        else
        {
            await Open(chatId, directoryId);
            await _botClient.SendText(chatId, "File added successfully!");
        }
    }

    [TgButtonCallback("ren")]
    public async Task MenuBtn_RenameDirectory(
        long chatId,
        IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Enter new directory name:");
    }

    [TgMessageResponse("ren")]
    public async Task RenameDirectory(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.Text == null)
        {
            await _botClient.SendText(
                chatId,
                "Please, send text message with new directory name");
            return;
        }

        var directoryId = long.Parse(parameters.First());
        await _directoryService.RenameDirectory(chatId, directoryId, message.Text);
        await Open(chatId, directoryId);
        await _botClient.SendText(chatId, "Renamed successfully!");
    }

    [TgButtonCallback("ga")]
    public async Task MenuBtn_GiveAccess(long chatId, IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send id of user to give access to:");
    }

    [TgMessageResponse("ga")]
    public async Task GiveAccess(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        var directoryId = long.Parse(parameters.First());
        await _directoryService.SetAccessRights(directoryId,
            (long)message.SenderId!,
            true,
            true);
        await Open(chatId, directoryId);
        await _botClient.SendText(chatId, "Successfully changed access rights!");
    }

    [TgButtonCallback("as")]
    public async Task MenuBtn_AddSubdir(long chatId, IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "Send name of the new directory:");
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
                "Please, send text message with a name for the new directory");
            return;
        }

        var parentId = long.Parse(parameters.First());
        var newDirectory = new DirectoryDto
        {
            Name = message.Text, OwnerId = (long)message.SenderId!, ParentId = parentId
        };
        await _directoryService.AddDirectory(chatId, newDirectory);
        await Open(chatId, parentId);
    }

    [TgButtonCallback("os")]
    public async Task MenuBtn_OpenSubdir(long chatId, IEnumerable<string> parameters)
    {
        var directoryId = long.Parse(parameters.First());
        await Open(chatId, directoryId);
    }

    [TgButtonCallback("of")]
    public async Task MenuBtn_OpenFile(long chatId, IEnumerable<string> parameters)
    {
        var fileId = long.Parse(parameters.First());
        await _redirectHandler.Redirect(chatId, typeof(FileMenu), fileId);
    }

    [TgButtonCallback("back")]
    public async Task MenuBtn_GoBack(long chatId, IEnumerable<string> parameters)
    {
        var parentId = long.Parse(parameters.First());
        if (parentId == 0)
        {
            await _redirectHandler.Redirect(chatId, typeof(RootMenu));
        }
        else
        {
            await Open(chatId, parentId);
        }
    }

    private TgKeyboard GetKeyboard(
        DirectoryDto dir,
        IEnumerable<DirectoryDto> subdirs,
        IEnumerable<FileDto> files)
    {
        var buttons = new List<TgMenuButton>();
        foreach (var subdir in subdirs)
        {
            buttons.Add(new TgMenuButton(subdir.Name, MenuBtn_OpenSubdir, subdir.Id));
        }

        foreach (var file in files)
        {
            buttons.Add(new TgMenuButton(file.Name, MenuBtn_OpenFile, file.Id));
        }

        buttons.AddRange(new List<TgMenuButton>
        {
            new("Rename", MenuBtn_RenameDirectory, dir.Id),
            //new("Give access", MenuBtn_GiveAccess, dir.Id),
            new("Add subdirectory", MenuBtn_AddSubdir, dir.Id),
            new("Add file", MenuBtn_AddFile, dir.Id)
        });
        buttons.Add(new TgMenuButton("Back", MenuBtn_GoBack, dir.ParentId ?? 0));
        var keyboard = GetKeyboard(buttons);
        return keyboard;
    }

    public async Task Open(long chatId, long directoryId)
    {
        var dir = await _directoryService.GetDirectory(chatId, directoryId);
        var subdirs = await _directoryService.GetChildren(chatId, directoryId);
        var files = await _fileService.GetFiles(chatId, directoryId);
        var keyboard = GetKeyboard(dir, subdirs, files);
        await _botClient.SendMenu(chatId, new MenuData(dir.Name, keyboard));
    }
}
