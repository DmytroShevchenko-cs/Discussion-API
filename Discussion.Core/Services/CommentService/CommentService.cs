namespace Discussion.Core.Services.CommentService;

using DTO.Comment.AddComment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;
using Infrastructure.Managers.RabbitProducer;
using MediatR;
using Microsoft.Extensions.Logging;

public class CommentService(
    IMediator mediator,
    ILogger<CommentService> logger,
    IRabbitProducer rabbitProducer
    ) : ICommentService
{
    public async Task<Result> AddCommentAsync(AddCommentRequestDTO request)
    {
        try
        {

            return Result.Success("Comment added successfully!");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while executing {nameof(AddCommentAsync)}");
            return Result.Failure($"Error while executing {nameof(AddCommentAsync)}");
        }
    }

    public async Task<Result<GetCommentsResponseDTO>> GetCommentsAsync(GetCommentsRequestDTO request)
    {
        try
        {

            return Result<GetCommentsResponseDTO>.Success(new GetCommentsResponseDTO());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result<GetCommentsResponseDTO>.Failure($"Error while executing {nameof(GetCommentsAsync)}");
        }
    }

    public async Task<Result> DeleteCommentAsync(int commentId)
    {
        try
        {

            return Result.Success("Comment was deleted!");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result.Failure($"Error while executing {nameof(DeleteCommentAsync)}");
        }
    }
}