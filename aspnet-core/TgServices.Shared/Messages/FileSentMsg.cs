namespace TgDrive.Messaging.RabbitMQ;

public class FileSentMsg
{
    public bool Success { get; set; }
    public long NewMessageId { get; set; }
    public string? ErrorMsg { get; set; }
}