namespace DriveServices.Messages;

public class SendFileMsg
{
    public long FileId { get; set; }
    public long ToChatId { get; set; }
    public long UserId { get; set; }
    
    public static string QueueName = "SendFileMsg";
}