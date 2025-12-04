namespace Discussion.Core.DTO.Comment.GetComments;

using NodaTime;

public class CommentItemModel
{
    public int Id { get; set; }
    public Instant CreatedAt { get; set; }
    public string Text { get; set; } = null!;
    public int? HeadCommentId { get; set; }
    public UserItemModel User { get; set; } = null!;
    public List<CommentItemModel> ReplyComments { get; set; } = null!;
    public List<string> ImagePaths { get; set; } = null!;
    public List<string> FilePaths { get; set; } = null!;
}