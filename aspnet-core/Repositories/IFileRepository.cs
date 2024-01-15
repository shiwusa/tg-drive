﻿using DataTransfer.Objects;

namespace Repositories;

public interface IFileRepository
{
    Task<FileDto> AddFile(FileDto file);
    Task<FileDto> GetFile(long fileId);

    Task<IEnumerable<FileDto>> GetFiles(
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<FileDto>> GetFilesByName(
        string name,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<IEnumerable<FileDto>> GetFilesByDescription(
        string description,
        long? directoryId = null,
        int? skip = null,
        int? take = null);

    Task<FileDto> ChangeDescription(long fileId, string newDescription);
    Task<FileDto> ChangeName(long fileId, string newName);
    Task<FileDto> Remove(long fileId);
}
