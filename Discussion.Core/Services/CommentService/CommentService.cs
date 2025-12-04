namespace Discussion.Core.Services.CommentService;

using DTO.Comment.AddComment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;
using Infrastructure.Managers.RabbitProducer;
using Infrastructure.Messages;
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
            var images = new List<FileMessageItemModel>();
            foreach (var image in request.Images)
            {
                using var ms = new MemoryStream();
                await image.CopyToAsync(ms);
                images.Add(new FileMessageItemModel
                {
                    FileName = image.FileName,
                    Content = ms.ToArray()
                });
            }

            var attachments = new List<FileMessageItemModel>();
            foreach (var file in request.Attachments)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                attachments.Add(new FileMessageItemModel
                {
                    FileName = file.FileName,
                    Content = ms.ToArray()
                });
            }
            
            await rabbitProducer.PublishAsync(new CommentMessageDTO
            {
                ParentId = request.ParentId,
                UserName = request.UserName,
                Email = request.Email,
                Text = request.Text,
                Images = images,
                Attachments = attachments
            });
            
            return Result.Success("Comment processing started!");
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