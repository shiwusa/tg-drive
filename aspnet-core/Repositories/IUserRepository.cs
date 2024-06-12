using TgDrive.Domain.Shared;

namespace TgDrive.DataAccess.Shared;

public interface IUserRepository
{
    Task<UserInfoDto?> GetUserInfo(long userId);
    Task<UserInfoDto> SetUserInfo(UserInfoDto userInfo);
}
