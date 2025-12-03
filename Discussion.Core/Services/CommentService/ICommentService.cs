namespace Discussion.Core.Services.CommentService;

using DTO.Comment.AddComment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;

public interface ICommentService
{
    Task<Result> AddCommentAsync(AddCommentRequestDTO request);
    Task<Result<GetCommentsResponseDTO>> GetCommentsAsync(GetCommentsRequestDTO request);
    Task<Result> DeleteCommentAsync(int commentId);
}