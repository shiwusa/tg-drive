namespace TgDrive.DataAccess.EntityFrameworkCore.Entities;

public class FileEntity
{
    public FileEntity()
    {
        Directory = default!;
        Name = default!;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime AccessTime { get; set; }
    public long AddedByUserId { get; set; }
    public UserInfoEntity Owner { get; set; }
    public long MessageId { get; set; }
    public long ChannelId { get; set; }
    public string? ReadAccessKey { get; set; }

    public long DirectoryId { get; set; }
    public DirectoryEntity Directory { get; set; }
}
