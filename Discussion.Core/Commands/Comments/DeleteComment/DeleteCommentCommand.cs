namespace Discussion.Core.Commands.Comments.DeleteComment;

using Database;
using Infrastructure.CQRS;
using Infrastructure.Result;
using MediatR;
using Microsoft.Extensions.Logging;

public sealed class DeleteCommentCommand : ICommand<Result>
{
    public int CommentId { get; set; }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
{
    private readonly ILogger<DeleteCommentCommandHandler> _logger;
    private readonly BaseDbContext _dbContext;

    public DeleteCommentCommandHandler(
        ILogger<DeleteCommentCommandHandler> logger,
        BaseDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Failure($"Error while executing {nameof(DeleteCommentCommand)}");
        }
    }
}