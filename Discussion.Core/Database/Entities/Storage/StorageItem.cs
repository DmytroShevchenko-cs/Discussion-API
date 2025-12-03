namespace Discussion.Core.Database.Entities.Storage;

using System.ComponentModel.DataAnnotations;
using Base;

public class StorageItem : BaseSimpleEntity
{
    public string BaseName { get; set; } = null!;
    public string FileName { get; set; } = null!;
    [StringLength(64)]
    public string ItemHash { get; set; } = null!;
    public string Extension { get; set; } = null!;
    [StringLength(250)]
    public string StoragePath { get; set; } = null!;
    public long? FileSize { get; set; }
}