namespace TvJahnOrchesterApp.Application.Common.Models;

public class FileStorage
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long FileSize { get; set; }
    public Guid? ByteaId { get; set; }
    public uint? LargeObjectId { get; set; }
    public FileStorageType StorageType { get; set; }
    public DateTime UploadDate { get; set; }
    public DateTime? LastAccessDate { get; set; }

    public virtual FileDataBytea FileDataBytea { get; set; }
}