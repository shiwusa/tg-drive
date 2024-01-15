namespace DriveServices.Messages;

public class FileSentMsg
{
    public bool Success { get; set; }
    public long NewMessageId { get; set; }
    public string? ErrorMsg { get; set; }
}