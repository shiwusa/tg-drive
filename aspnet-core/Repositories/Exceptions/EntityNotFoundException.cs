﻿namespace TgDrive.DataAccess.Shared;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, long fileId)
        : base($"{entityName} with ID={fileId} does not exist!")
    {
    }
}
