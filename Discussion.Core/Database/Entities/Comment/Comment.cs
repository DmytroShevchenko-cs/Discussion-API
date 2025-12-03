namespace Discussion.Core.Database.Entities.Comment;

using Base;
using User;

public class Comment : BaseEntity
{
    public string Text { get; set; } = null!;

    public int? HeadCommentId { get; set; }
    public Comment? HeadComment { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<Comment> ReplyComments { get; set; } = null!;
    
    public ICollection<CommentImage> Images { get; set; } = null!;
    public ICollection<CommentFile> Files { get; set; } = null!;
}