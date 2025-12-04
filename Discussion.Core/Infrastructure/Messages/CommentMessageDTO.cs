namespace Discussion.Core.Infrastructure.Messages;

public record CommentMessageDTO
{
    public int? ParentId { get; set; }
    
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Text { get; set; } = null!;
    
    public List<FileMessageItemModel> Images { get; set; } = [];
    
    public List<FileMessageItemModel> Attachments { get; set; } = [];
};
