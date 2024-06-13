using TgDrive.Domain.Shared;

namespace TgDrive.Infrastructure.RabbitMQ;

public interface ITgFileServiceClient
{
    Task<FileDto?> AddFile(long userId, FileDto file);
    
    Task<long> SendFile(long userId, long fileId, long toChatId);

    Task<IEnumerable<long>> SendFiles(
        long userId,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<long>> SendFilesByName(
        long userId,
        string name,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<long>> SendFilesByDescription(
        long userId,
        string description,
        long? directoryId = null,
        int? skip = null,
        int? take = null);
}
