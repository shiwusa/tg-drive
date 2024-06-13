using MassTransit;
using TgDrive.Domain.Shared;

namespace TgDrive.Infrastructure.RabbitMQ;

public class TgFileServiceClient : ITgFileServiceClient
{
    private readonly IRequestClient<SendFileMsg> _sendFileClient;

    public TgFileServiceClient(IRequestClient<SendFileMsg> sendFileClient)
    {
        _sendFileClient = sendFileClient;
    }

    public Task<FileDto?> AddFile(long userId, FileDto file)
    {
        throw new NotImplementedException();
    }

    public async Task<long> SendFile(long fileId, long toChatId)
    {
        var msg = new SendFileMsg
        {
            FileId = fileId,
            ToChatId = toChatId,
        };
        var response = await _sendFileClient.GetResponse<FileSentMsg>(msg, timeout: RequestTimeout.After(m: 1));
        if (!response.Message.Success)
        {
            throw new Exception(response.Message.ErrorMsg);
        }

        return response.Message.NewMessageId;
    }

    public async Task<long> SendFile(long userId, long fileId, long toChatId)
    {
        var msg = new SendFileMsg
        {
            FileId = fileId,
            ToChatId = toChatId,
            UserId = userId,
        };
        var response = await _sendFileClient.GetResponse<FileSentMsg>(msg, timeout: RequestTimeout.After(m: 1));
        if (!response.Message.Success)
        {
            throw new Exception(response.Message.ErrorMsg);
        }

        return response.Message.NewMessageId;
    }

    public Task<IEnumerable<long>> SendFiles(long userId, long? directoryId = null, int? skip = null, int? take = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<long>> SendFilesByDescription(long userId, string description, long? directoryId = null, int? skip = null, int? take = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<long>> SendFilesByName(long userId, string name, long? directoryId = null, int? skip = null, int? take = null)
    {
        throw new NotImplementedException();
    }
}
