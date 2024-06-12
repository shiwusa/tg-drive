using TgDrive.Domain.Shared;
using TgDrive.DataAccess.Shared;

namespace TgDrive.Domain.Services.Implementations;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IDirectoryRepository _directoryRepository;

    public FileService(
        IFileRepository fileRepository,
        IDirectoryRepository directoryRepository)
    {
        _fileRepository = fileRepository;
        _directoryRepository = directoryRepository;
    }

    public async Task<FileDto> ChangeDescription(
        long userId,
        long fileId,
        string newDescription)
    {
        var file = await _fileRepository.GetFile(fileId);
        if (userId != file.AddedByUserId)
        {
            throw new InvalidOperationException(
                "You do not have access to this file.");
        }

        var edited = await _fileRepository.ChangeDescription(fileId, newDescription);
        return edited;
    }

    public async Task<FileDto> ChangeName(long userId, long fileId, string newName)
    {
        var file = await _fileRepository.GetFile(fileId);
        if (userId != file.AddedByUserId)
        {
            throw new InvalidOperationException(
                "You do not have access to this file.");
        }

        var edited = await _fileRepository.ChangeName(fileId, newName);
        return edited;
    }

    public async Task<FileDto> Remove(long userId, long fileId)
    {
        var file = await _fileRepository.GetFile(fileId);
        if (userId != file.AddedByUserId)
        {
            throw new InvalidOperationException(
                "You do not have access to this file.");
        }

        var removed = await _fileRepository.Remove(fileId);
        return removed;
    }

    public async Task<FileDto> GetFile(long userId, long fileId)
    {
        var file = await _fileRepository.GetFile(fileId);
        if (userId != file.AddedByUserId)
        {
            throw new InvalidOperationException(
                "You do not have access to this file.");
        }

        return file;
    }

    public async Task<FileDto> AddFile(long userId, FileDto file)
    {
        if (userId != file.AddedByUserId)
        {
            throw new InvalidOperationException(
               "You cannot add this file.");
        }

        return await _fileRepository.AddFile(file);
    }

    public async Task<IEnumerable<FileDto>> GetFiles(
        long userId,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var directory = await _directoryRepository.GetDirectory(
            (long)directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
               "You do not have access to these files.");
        }

        var files = await _fileRepository.GetFiles(directoryId, skip, take);
        return files;
    }

    public async Task<IEnumerable<FileDto>> GetFilesByName(
        long userId,
        string name,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var directory = await _directoryRepository.GetDirectory((long)directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
               "You do not have access to these files.");
        }

        var files = await _fileRepository.GetFilesByName(
            name, directoryId, skip, take);
        return files;
    }

    public async Task<IEnumerable<FileDto>> GetFilesByDescription(
        long userId,
        string description,
        long? directoryId = null,
        int? skip = null,
        int? take = null)
    {
        var directory = await _directoryRepository.GetDirectory((long)directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
               "You do not have access to these files.");
        }

        var files = await _fileRepository.GetFilesByName(
            description, directoryId, skip, take);
        return files;
    }
}
