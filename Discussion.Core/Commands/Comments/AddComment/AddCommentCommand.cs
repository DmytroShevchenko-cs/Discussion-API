namespace Discussion.Core.Commands.Comments.AddComment;

using Database;
using Infrastructure.CQRS;
using Infrastructure.Result;
using MediatR;
using Microsoft.Extensions.Logging;

public sealed class AddCommentCommand : ICommand<Result>
{
    
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
{
    private readonly ILogger<AddCommentCommandHandler> _logger;
    private readonly BaseDbContext _dbContext;

    public AddCommentCommandHandler(
        ILogger<AddCommentCommandHandler> logger,
        BaseDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        AddCommentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Failure($"Error while executing {nameof(AddCommentCommand)}");
        }
    }
}