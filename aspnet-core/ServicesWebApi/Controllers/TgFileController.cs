using DataTransfer.Objects;
using DriveServices;
using Microsoft.AspNetCore.Mvc;

namespace ServicesWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TgFileController : TgDriveController
{
    private readonly ILogger<TgFileController> _logger;
    private readonly ITgFileService _tgFileService;

    public TgFileController(
        ILogger<TgFileController> logger,
        ITgFileService tgFileService)
    {
        _logger = logger;
        _tgFileService = tgFileService;
    }

    [HttpPost("SendFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> SendFile(
        [FromQuery] long fileId)
    {
        var sentFile = await _tgFileService.SendFile(UserId, fileId, UserId);
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
        var sentFileIds = await _tgFileService.SendFiles(
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
        var sentFileIds = await _tgFileService.SendFilesByName(
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
        var sentFileIds = await _tgFileService.SendFilesByDescription(
            UserId, description, directoryId, skip, take);
        if (sentFileIds is null)
        {
            return BadRequest(
                new ArgumentNullException(nameof(sentFileIds)));
        }

        return Ok(sentFileIds);
    }
}
