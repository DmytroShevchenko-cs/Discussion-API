namespace Discussion.Core.DTO.Hub.Comments.NewComment;

public class UserItemModel
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}