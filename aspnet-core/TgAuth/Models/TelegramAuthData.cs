namespace TgAuth.Models;

public class TelegramAuthData
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhotoUrl { get; set; }
    public string AuthDate { get; set; }
}