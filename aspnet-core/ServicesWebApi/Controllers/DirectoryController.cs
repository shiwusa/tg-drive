using DataTransfer.Objects;
using DriveServices;
using Microsoft.AspNetCore.Mvc;
using Repositories.Exceptions;

namespace ServicesWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectoryController : TgDriveController
{
    private readonly ILogger<DirectoryController> _logger;
    private readonly IDirectoryService _directoryService;

    public DirectoryController(
        ILogger<DirectoryController> logger,
        IDirectoryService directoryService)
    {
        _logger = logger;
        _directoryService = directoryService;
    }

    [HttpPost("AddDirectory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DirectoryDto>> AddDirectory(
        [FromBody] DirectoryDto? directory)
    {
        if (directory is null)
        {
            return BadRequest(
                new ArgumentNullException(nameof(directory)));
        }

        var writtenDir = await _directoryService.AddDirectory(UserId, directory);
        return CreatedAtAction(nameof(GetDirectory),
            new { directoryId = writtenDir.Id }, writtenDir);
    }

    [HttpGet("GetDirectory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DirectoryDto>> GetDirectory(
        [FromQuery] long directoryId)
    {
        var directory = await _directoryService.GetDirectory(UserId, directoryId);
        return directory is null ? NotFound() : Ok(directory);
    }

    [HttpGet("GetChildren")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DirectoryDto>>> GetChildren(
        [FromQuery] long directoryId)
    {
        var children = await _directoryService.GetChildren(UserId, directoryId);
        return children is null ? NotFound() : Ok(children);
    }

    [HttpGet("GetRoot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DirectoryDto>>> GetRoot()
    {
        var rootDirs = await _directoryService.GetRoot(UserId);
        return rootDirs is null ? NotFound() : Ok(rootDirs);
    }

    [HttpPut("RenameDirectory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DirectoryDto>> RenameDirectory(
        [FromQuery] long directoryId,
        [FromQuery] string newName)
    {
        var directory = await _directoryService.RenameDirectory(
            UserId, directoryId, newName);
        if (directory is null)
        {
            return BadRequest(
                new TgDirectoryNotFoundException(directoryId));
        }

        return Ok(directory);
    }

    [HttpDelete("RemoveDirectory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DirectoryDto>> RemoveDirectory(
        [FromQuery] long directoryId)
    {
        var directory = await _directoryService.Remove(UserId, directoryId);
        if (directory is null)
        {
            return BadRequest(
                new TgDirectoryNotFoundException(directoryId));
        }

        return Ok(directory);
    }
}
