namespace TgDrive.DataAccess.EntityFrameworkCore.Entities;

public class DirectoryEntity
{
    public DirectoryEntity()
    {
        Name = default!;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public long OwnerId { get; set; }
    public UserInfoEntity Owner { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime AccessTime { get; set; }
    public string? ReadAccessKey { get; set; }
    public string? WriteAccessKey { get; set; }
    public long? ParentId { get; set; }
    public DirectoryEntity? Parent { get; set; }
    public bool Leaf { get; set; }
    
    public ICollection<DirectoryEntity> Children { get; } = new List<DirectoryEntity>();
    public ICollection<FileEntity> Files { get; } = new List<FileEntity>();
    public ICollection<DirectoryAccess> Accesses { get; } = new List<DirectoryAccess>();
}
