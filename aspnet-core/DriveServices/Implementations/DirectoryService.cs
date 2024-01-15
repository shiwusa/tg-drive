using DataTransfer.Objects;
using Repositories;

namespace DriveServices.Implementations;

public class DirectoryService : IDirectoryService
{
    private readonly IDirectoryRepository _directoryRepository;

    public DirectoryService(IDirectoryRepository directoryRepository)
    {
        _directoryRepository = directoryRepository;
    }

    public async Task<DirectoryDto> AddDirectory(long userId, DirectoryDto directory)
    {
        if (directory.ParentId != null)
        {
            var parentDir = await _directoryRepository.GetDirectory(
                (long)directory.ParentId);
            if (parentDir.OwnerId != userId)
            {
                throw new InvalidOperationException(
                    "You do not have access to this directory.");
            }

            if (parentDir.Leaf)
            {
                await _directoryRepository.SetDirectoryNotLeaf(parentDir.Id);
            }
        }
        
        directory.Leaf = true;
        directory.OwnerId = userId;
        directory.ReadAccessKey = null;
        directory.WriteAccessKey = null;
        var added = await _directoryRepository.AddDirectory(directory);
        return added;
    }

    public async Task<DirectoryDto> GetDirectory(long userId, long directoryId)
    {
        var directory = await _directoryRepository.GetDirectory(directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
                "You do not have access to this directory.");
        }

        return directory;
    }

    public async Task<IEnumerable<DirectoryDto>> GetChildren(long userId, long directoryId)
    {
        var directory = await _directoryRepository.GetDirectory(directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
                "You do not have access to this directory.");
        }

        var children = await _directoryRepository.GetChildren(directoryId);
        return children;
    }

    public async Task<IEnumerable<DirectoryDto>> GetRoot(long userId)
    {
        var userRoot = await _directoryRepository.GetRoot(userId);
        return userRoot;
    }

    public async Task<DirectoryDto> RenameDirectory(long userId, long directoryId, string newName)
    {
        var directory = await _directoryRepository.GetDirectory(directoryId);
        if (directory.OwnerId != userId)
        {
            throw new InvalidOperationException(
                "You do not have access to this directory.");
        }

        var renamed = await _directoryRepository.RenameDirectory(directoryId, newName);
        return renamed;
    }

    public async Task<DirectoryDto> Remove(long userId, long directoryId)
    {
        var directory = await _directoryRepository.GetDirectory(directoryId);
        if (directory.ParentId != null)
        {
            var parentDir = await _directoryRepository.GetDirectory(
                (long)directory.ParentId);
            if (parentDir.OwnerId != userId)
            {
                throw new InvalidOperationException(
                    "You do not have access to this directory.");
            }
        }

        var removed = await _directoryRepository.Remove(directoryId);
        return removed;
    }

    public async Task SetAccessRights(
        long directoryId,
        long userId,
        bool? read,
        bool? write)
    {
        await _directoryRepository.SetAccessRights(directoryId, userId, read, write);
    }
}
