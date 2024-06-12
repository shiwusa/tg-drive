namespace TgDrive.DataAccess.Shared;

public class TgDirectoryNotFoundException : EntityNotFoundException
{
    public TgDirectoryNotFoundException(long directoryId)
        : base("Directory", directoryId)
    {
    }
}
