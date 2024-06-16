using TgDrive.Domain.Telegram.Updates;

namespace TgDrive.Domain.Telegram.Abstractions;

public interface ITgDriveUpdateHandler
{
    Task HandleCallback(TgCallbackUpdate update);
    Task HandleMessage(TgMessageUpdate update);
    Task HandleCommand(TgCommandUpdate update);
    Task HandleError(Exception error);
}
