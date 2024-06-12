using TgDrive.Domain.Shared;

namespace TgDrive.Domain.Services;

public interface IUserService
{
    Task<UserInfoDto?> GetUserInfo(long userId);
    Task<UserInfoDto> SetUserInfo(UserInfoDto userInfo);
}
