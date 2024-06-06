using DataTransfer.Objects;
using DriveServices;
using Microsoft.AspNetCore.Mvc;
using Repositories.Exceptions;

namespace TgDrive.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : TgDriveController
{
    private readonly ILogger<FileController> _logger;
    private readonly IFileService _fileService;

    public FileController(
        ILogger<FileController> logger,
        IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    [HttpPut("ChangeDescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileDto>> ChangeDescription(
        [FromQuery] long fileId,
        [FromQuery] string newDescription)
    {
        var file = await _fileService.ChangeDescription(
            UserId, fileId, newDescription);
        if (file is null)
        {
            return BadRequest(
                new TgFileNotFoundException(fileId));
        }

        return Ok(file);
    }

    [HttpPut("ChangeName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileDto>> ChangeName(
        [FromQuery] long fileId,
        [FromQuery] string newName)
    {
        var file = await _fileService.ChangeName(UserId, fileId, newName);
        if (file is null)
        {
            return BadRequest(
                new TgFileNotFoundException(fileId));
        }

        return Ok(file);
    }

    [HttpDelete("Remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileDto>> Remove(
        [FromQuery] long fileId)
    {
        var file = await _fileService.Remove(UserId, fileId);
        if (file is null)
        {
            return BadRequest(
                new TgFileNotFoundException(fileId));
        }

        return Ok(file);
    }

    [HttpGet("GetFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileDto>> GetFile(
        [FromQuery] long fileId)
    {
        var file = await _fileService.GetFile(UserId, fileId);
        return file is null ? NotFound() : Ok(file);
    }

    [HttpGet("GetFiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<FileDto>>> GetFiles(
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var files = await _fileService.GetFiles(
            UserId, directoryId, skip, take);
        return files is null ? NotFound() : Ok(files);
    }

    [HttpGet("GetFilesByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<FileDto>>> GetFilesByName(
        [FromQuery] string name,
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var files = await _fileService.GetFilesByName(
            UserId, name, directoryId, skip, take);
        return files is null ? NotFound() : Ok(files);
    }

    [HttpGet("GetFilesByDescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<FileDto>>> GetFilesByDescription(
        [FromQuery] string description,
        [FromQuery] long? directoryId = null,
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var files = await _fileService.GetFilesByName(
            UserId, description, directoryId, skip, take);
        return files is null ? NotFound() : Ok(files);
    }
}
