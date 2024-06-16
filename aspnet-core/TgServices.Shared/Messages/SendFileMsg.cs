namespace TgDrive.Infrastructure.RabbitMQ;

    public class SendFileMsg
    {
        public long FileId { get; set; }
        public long ToChatId { get; set; }
        public long UserId { get; set; }
    
        public const string QueueName = "SendFileMsg";
    }
