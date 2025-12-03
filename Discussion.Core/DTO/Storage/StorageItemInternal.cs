namespace Discussion.Core.DTO.Storage;

public class StorageItemInternal
{
    public string BaseName { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public long FileSize { get; set; }
    public string StoragePath { get; set; } = null!;
}