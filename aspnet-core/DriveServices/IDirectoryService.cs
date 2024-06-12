using TgDrive.Domain.Shared;

namespace TgDrive.Domain.Services;

public interface IDirectoryService
{
    Task<DirectoryDto> AddDirectory(long userId, DirectoryDto directory);
    Task<DirectoryDto> GetDirectory(long userId, long directoryId);
    Task<IEnumerable<DirectoryDto>> GetChildren(long userId, long directoryId);
    Task<IEnumerable<DirectoryDto>> GetRoot(long userId);
    Task<DirectoryDto> RenameDirectory(long userId, long directoryId, string newName);
    Task<DirectoryDto> Remove(long userId, long directoryId);
    Task SetAccessRights(long directoryId, long userId, bool? read, bool? write);
}
