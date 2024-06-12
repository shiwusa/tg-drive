namespace TgDrive.DataAccess.Shared;

public class TgUserInfoNotFoundException : EntityNotFoundException
{
    public TgUserInfoNotFoundException(long userInfoId)
        : base("User info", userInfoId)
    {
    }
}
