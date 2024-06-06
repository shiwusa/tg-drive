using Microsoft.AspNetCore.Mvc;
using TgDrive.Web.Models;

namespace TgDrive.Web;

public class TgDriveController : ControllerBase
{
    public TelegramAuthData AuthData =>
        HttpContext.Items["auth-data"] is TelegramAuthData authData
            ? authData
            : throw new Exception("Auth data not provided.");

    public long UserId => AuthData.Id;
}