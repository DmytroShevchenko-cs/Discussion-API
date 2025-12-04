namespace Discussion.Core.Commands.Comments.DeleteComment;

using Database;
using Infrastructure.Common.CQRS;
using Infrastructure.Common.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class DeleteCommentCommand : ICommand<Result>
{
    public int CommentId { get; set; }
}

public class DeleteCommentCommandHandler(
    ILogger<DeleteCommentCommandHandler> logger,
    BaseDbContext dbContext)
    : IRequestHandler<DeleteCommentCommand, Result>
{
    public async Task<Result> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.Comments
                .Where(r => r.Id == request.CommentId)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(p => p.IsDeleted, true), 
                    cancellationToken);
            
            return Result.Success("Comment deleted!");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result.Failure($"Error while executing {nameof(DeleteCommentCommand)}");
        }
    }
}