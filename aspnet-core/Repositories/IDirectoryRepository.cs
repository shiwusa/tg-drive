using TgDrive.Domain.Shared;

namespace TgDrive.DataAccess.Shared;

public interface IDirectoryRepository
{
    Task<DirectoryDto> AddDirectory(DirectoryDto directory);
    Task<DirectoryDto> GetDirectory(long directoryId);
    Task<IEnumerable<DirectoryDto>> GetChildren(long directoryId);
    Task<IEnumerable<DirectoryDto>> GetRoot(long userId);
    Task<DirectoryDto> RenameDirectory(long directoryId, string newName);
    Task<bool> SetDirectoryNotLeaf(long directoryId);
    Task<DirectoryDto> Remove(long directoryId);
    Task SetAccessRights(long directoryId, long userId, bool? read, bool? write);
    Task<bool> DoesUserHaveWriteAccess(long directoryId, long userId);
    Task<bool> DoesUserHaveReadAccess(long directoryId, long userId);
}
