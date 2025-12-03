namespace Discussion.Core.Database.Entities.Comment;

using Base;
using Storage;

public class CommentImage : BaseSimpleEntity
{
    public int CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
    
    public int FileId { get; set; }
    public StorageItem StorageItem { get; set; } = null!;
}