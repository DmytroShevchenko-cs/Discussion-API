namespace Discussion.Core.Queries.Comments.GetComments;

using DTO.Comment.GetComments;

public sealed class GetCommentsQueryResult
{
    public int Count { get; set; }
    public List<CommentItemModel> Items { get; set; } = null!;
}