namespace Discussion.Core.Services.CommentService;

using DTO.Comment.AddComment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;
using Queries.Comments.GetComments;

public interface ICommentService
{
    Task<Result> AddCommentAsync(AddCommentRequestDTO request);
    Task<Result<GetCommentsQueryResult>> GetCommentsAsync(GetCommentsRequestDTO request);
    Task<Result> DeleteCommentAsync(int commentId);
}