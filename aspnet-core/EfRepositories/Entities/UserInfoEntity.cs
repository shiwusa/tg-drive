namespace TgDrive.DataAccess.EntityFrameworkCore.Entities;

public class UserInfoEntity
{
    public long Id { get; set; }
    public long? StorageChannelId { get; set; }

    public ICollection<FileEntity> Files { get; } = new List<FileEntity>();
    public ICollection<DirectoryEntity> Directories { get; } = new List<DirectoryEntity>();
}
