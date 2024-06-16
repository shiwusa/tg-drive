using TgDrive.Infrastructure.RabbitMQ;
using MassTransit;

namespace TgDrive.Domain.Services;

public class TgDriveEventService : IConsumer<SendFileMsg>
{
    private readonly ITgFileService _tgFileService;

    public TgDriveEventService(ITgFileService tgFileService)
    {
        _tgFileService = tgFileService;
    }

    public async Task Consume(ConsumeContext<SendFileMsg> context)
    {
        var response = new FileSentMsg
        {
            Success = false,
            ErrorMsg = null,
            NewMessageId = 0,
        };

        try
        {
            response.NewMessageId = await _tgFileService.SendFile(context.Message.UserId,
                context.Message.FileId,
                context.Message.ToChatId);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMsg = ex.Message;
        }
        finally
        {
            await context.RespondAsync(response);
        }
    }
}