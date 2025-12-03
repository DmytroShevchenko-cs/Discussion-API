namespace Discussion.Core.DTO.Hub.Comments.NewComment;

public class NewCommentDTO
{
    public int? HeadCommentId { get; set; }
    public string Text { get; set; } = null!;
    public string CreatedAt { get; set; } = null!;
    public UserItemModel User { get; set; } = null!;
    public List<string> Images { get; set; } = null!;
    public List<string> Files { get; set; } = null!;
}