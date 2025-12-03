namespace Discussion.Core.Database.Entities.User;

using Base;
using Comment;

public class User : BaseSimpleEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public ICollection<Comment> Comments { get; set; } = null!;
}