namespace Discussion.Core.Services.CommentService;

using Commands.Comments.DeleteComment;
using DTO.Comment.AddComment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;
using Infrastructure.Managers.RabbitProducer;
using Infrastructure.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using Queries.Comments.GetComments;

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

    public async Task<Result<GetCommentsQueryResult>> GetCommentsAsync(GetCommentsRequestDTO request)
    {
        try
        {
            var result = await mediator.Send(new GetCommentsQuery()
            {
                Offset = request.Offset,
                PageSize = request.PageSize
            });
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result<GetCommentsQueryResult>.Failure($"Error while executing {nameof(GetCommentsAsync)}");
        }
    }

    public async Task<Result> DeleteCommentAsync(int commentId)
    {
        var result = await mediator.Send(new DeleteCommentCommand
        {
            CommentId = commentId
        });
            
        return result;
    }
}