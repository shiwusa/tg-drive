using DataTransfer.Objects;

namespace DriveServices;

public interface IFileService
{
    Task<FileDto> ChangeDescription(long userId, long fileId, string newDescription);
    Task<FileDto> ChangeName(long userId, long fileId, string newName);
    Task<FileDto> Remove(long userId, long fileId);
    Task<FileDto> GetFile(long userId, long fileId);
    Task<FileDto> AddFile(long userId, FileDto file);

    Task<IEnumerable<FileDto>> GetFiles(
        long userId,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<FileDto>> GetFilesByName(
        long userId,
        string name,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<FileDto>> GetFilesByDescription(
        long userId,
        string description,
        long? directoryId = null,
        int? skip = null,
        int? take = null);
}
