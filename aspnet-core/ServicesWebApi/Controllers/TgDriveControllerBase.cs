using Microsoft.AspNetCore.Mvc;
using TgDrive.Web.Auth;

namespace TgDrive.Web.HttpApi;

public class TgDriveControllerBase : ControllerBase
{
    public TelegramAuthData AuthData =>
        HttpContext.Items[AuthorizationConsts.AuthDataItemName] is TelegramAuthData authData
            ? authData
            : throw new Exception("Auth data not provided.");

    public long UserId => AuthData.Id;
}
