namespace Discussion.Core.DTO.Comment.AddComment;

using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;

public class AddCommentRequestDTO
{
    public int? ParentId { get; set; }
    
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Text { get; set; } = null!;
    
    [FileValidator("jpg,jpeg,png,gif", isOptional: true)]
    public List<IFormFile> Images { get; set; } = [];
    [FileValidator("", isOptional: true)]
    public List<IFormFile> Attachments { get; set; } = [];
};