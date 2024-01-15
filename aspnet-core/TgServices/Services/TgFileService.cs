using DataTransfer.Objects;
using Repositories;
using System.IO;
using TgGateway.Abstractions;

namespace DriveServices.Implementations;

public class TgFileService : ITgFileService
{
    private readonly IFileService _fileService;
    private readonly IUserService _userService;
    private readonly IBotClient _botClient;

    public TgFileService(
        IFileService fileService,
        IUserService userService,
        IBotClient botClient)
    {
        _fileService = fileService;
        _userService = userService;
        _botClient = botClient;
    }

    public async Task<FileDto?> AddFile(long userId, FileDto file)
    {
        var userInfo = await _userService.GetUserInfo(file.AddedByUserId);
        if (userInfo?.StorageChannelId == null)
        {
            return null;
        }
        var forwardedId = await _botClient.CopyMessageUnmanaged(file.ChatId,
            (long)userInfo.StorageChannelId,
            file.MessageId);
        var forwardedFile = new FileDto
        {
            AddedByUserId = file.AddedByUserId,
            ChatId = (long)userInfo.StorageChannelId,
            Description = file.Description,
            DirectoryId = file.DirectoryId,
            MessageId = forwardedId,
            Name = file.Name,
            ReadAccessKey = file.ReadAccessKey
        };
        var savedFile = await _fileService.AddFile(userId, forwardedFile);
        return savedFile;
    }

    public async Task<long> SendFile(
        long userId,
        long fileId,
        long toChatId)
    {
        var file = await _fileService.GetFile(userId, fileId);
        var copiedId =
            await _botClient.CopyMessage(file.ChatId,
                toChatId,
                file.MessageId);
        return copiedId;
    }

    public async Task<IEnumerable<long>> SendFiles(
        long userId,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var files = await _fileService.GetFiles(
            userId, directoryId, skip, take);
        if (!files.Any())
        {
            return new List<long>();
        }

        var userInfo = await _userService.GetUserInfo(files.First()
            .AddedByUserId);

        var chatId = files.First()
            .Id;
        var fileMessageIds = files.Select(x => x.MessageId);
        var sent =
            await _botClient
                .CopyMessagesUnmanaged(chatId,
                    (long)userInfo.StorageChannelId,
                    fileMessageIds)
                .ToListAsync();
        return sent;
    }

    public async Task<IEnumerable<long>> SendFilesByName(
        long userId,
        string name,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var files = await _fileService.GetFilesByName(
            userId, name, directoryId, skip, take);
        if (!files.Any())
        {
            return new List<long>();
        }

        var userInfo = await _userService.GetUserInfo(files.First()
            .AddedByUserId);

        var chatId = files.First()
            .Id;
        var fileMessageIds = files.Select(x => x.MessageId);
        var sent =
            await _botClient
                .CopyMessagesUnmanaged(chatId,
                    (long)userInfo.StorageChannelId,
                    fileMessageIds)
                .ToListAsync();
        return sent;
    }

    public async Task<IEnumerable<long>> SendFilesByDescription(
        long userId,
        string description,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var files = await _fileService.GetFilesByDescription(
            userId, description, directoryId, skip, take);
        if (!files.Any())
        {
            return new List<long>();
        }

        var userInfo = await _userService.GetUserInfo(files.First()
            .AddedByUserId);
        
        var chatId = files.First()
            .Id;
        var fileMessageIds = files.Select(x => x.MessageId);
        var sent =
            await _botClient
                .CopyMessagesUnmanaged(chatId,
                    (long)userInfo.StorageChannelId,
                    fileMessageIds)
                .ToListAsync();
        return sent;
    }
}
