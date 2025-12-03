namespace Discussion.Core.Queries.Comments.GetComments;

using System;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Infrastructure.Result;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result<GetCommentsResult>>
{
    private readonly ILogger<GetCommentsQueryHandler> _logger;
    private readonly BaseDbContext _dbContext;

    public GetCommentsQueryHandler(
        ILogger<GetCommentsQueryHandler> logger,
        BaseDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<GetCommentsResult>> Handle(
        GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Result<GetCommentsResult>.Success(new GetCommentsResult{});
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result<GetCommentsResult>.Failure($"Error while executing {nameof(GetCommentsQuery)}");
        }
    }
}