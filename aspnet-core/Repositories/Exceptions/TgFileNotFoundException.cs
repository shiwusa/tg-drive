namespace TgDrive.DataAccess.Shared;

public class TgFileNotFoundException : EntityNotFoundException
{
    public TgFileNotFoundException(long fileId)
        : base("File", fileId)
    {
    }
}
