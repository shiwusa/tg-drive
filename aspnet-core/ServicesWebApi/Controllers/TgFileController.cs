using Microsoft.AspNetCore.Mvc;
using TgDrive.Infrastructure.RabbitMQ;

namespace TgDrive.Web.HttpApi;

[Route("api/[controller]")]
[ApiController]
public class TgFileController : TgDriveControllerBase
{
    private readonly ILogger<TgFileController> _logger;
    private readonly ITgFileServiceClient _tgFileServiceClient;

    public TgFileController(
        ILogger<TgFileController> logger,
        ITgFileServiceClient tgFileServiceClient)
    {
        _logger = logger;
        _tgFileServiceClient = tgFileServiceClient;
    }

    [HttpPost("SendFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> SendFile(
        [FromQuery] long fileId)
    {
        var sentFile = await _tgFileServiceClient.SendFile(UserId, fileId, UserId);
        return Ok(sentFile);
    }

    [HttpPost("SendFiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<long>>> SendFiles(
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var sentFileIds = await _tgFileServiceClient.SendFiles(
            UserId, directoryId, skip, take);
        if (sentFileIds is null)
        {
            return BadRequest(
                new ArgumentNullException(nameof(sentFileIds)));
        }

        return Ok(sentFileIds);
    }

    [HttpPost("SendFilesByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<long>>> SendFilesByName(
        [FromQuery] string name,
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var sentFileIds = await _tgFileServiceClient.SendFilesByName(
            UserId, name, directoryId, skip, take);
        if (sentFileIds is null)
        {
            return BadRequest(
                new ArgumentNullException(nameof(sentFileIds)));
        }

        return Ok(sentFileIds);
    }

    [HttpPost("SendFilesByDescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<long>>> SendFilesByDescription(
        [FromQuery] string description,
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var sentFileIds = await _tgFileServiceClient.SendFilesByDescription(
            UserId, description, directoryId, skip, take);
        if (sentFileIds is null)
        {
            return BadRequest(
                new ArgumentNullException(nameof(sentFileIds)));
        }

        return Ok(sentFileIds);
    }
}
