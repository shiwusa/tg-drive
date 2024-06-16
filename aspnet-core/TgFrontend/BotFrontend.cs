using TgDrive.Domain.Telegram.Abstractions;

namespace TgDrive.BotClient.Frontend;

public class BotFrontend
{
    private readonly ITgDriveBotClient _botClient;
    private readonly ITgDriveUpdateHandler _updateHandler;

    public BotFrontend(
        ITgDriveBotClient botClient,
        ITgDriveUpdateHandler updateHandler)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
    }

    public void Start()
    {
        _botClient.StartReceiving(_updateHandler);
    }
}
