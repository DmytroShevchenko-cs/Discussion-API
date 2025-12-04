namespace Discussion.Core.Queries.Comments.GetComments;

using System;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Database.Entities.Comment;
using DTO.Comment.GetComments;
using Infrastructure.Common.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.R2StorageService;

public class GetCommentsQueryHandler(
    ILogger<GetCommentsQueryHandler> logger,
    BaseDbContext dbContext,
    IR2StorageService r2Storage)
    : IRequestHandler<GetCommentsQuery, Result<GetCommentsQueryResult>>
{
    public async Task<Result<GetCommentsQueryResult>> Handle(
        GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var rootQuery = dbContext.Comments
                .AsNoTracking()
                .Where(c => c.HeadCommentId == null && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await rootQuery.CountAsync(cancellationToken);

            var rootComments = await rootQuery
                .Skip(request.Offset)
                .Take(request.PageSize)
                .Include(c => c.User)
                .Include(c => c.Images)
                .ThenInclude(r => r.StorageItem)
                .Include(c => c.Files)
                .ThenInclude(r => r.StorageItem)
                .ToListAsync(cancellationToken);

            if (!rootComments.Any())
            {
                return Result<GetCommentsQueryResult>.Success(new GetCommentsQueryResult
                {
                    Count = 0,
                    Items = new List<CommentItemModel>()
                });
            }

            var rootIds = rootComments.Select(c => c.Id).ToList();

            var replyComments = await dbContext.Comments
                .AsNoTracking()
                .Where(c => c.HeadCommentId != null && rootIds.Contains(c.HeadCommentId.Value) && !c.IsDeleted)
                .Include(c => c.User)
                .Include(c => c.Images)
                .ThenInclude(r => r.StorageItem)
                .Include(c => c.Files)
                .ThenInclude(r => r.StorageItem)
                .ToListAsync(cancellationToken);

            var repliesByParent = replyComments
                .GroupBy(c => c.HeadCommentId!.Value)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.CreatedAt).ToList());

            var items = new List<CommentItemModel>();
            foreach (var comment in rootComments)
            {
                var mapped = await MapCommentAsync(comment, repliesByParent);
                items.Add(mapped);
            }

            return Result<GetCommentsQueryResult>.Success(new GetCommentsQueryResult
            {
                Count = totalCount,
                Items = items
            });
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return Result<GetCommentsQueryResult>.Failure($"Error while executing {nameof(GetCommentsQuery)}");
        }
    }

    private async Task<CommentItemModel> MapCommentAsync(Comment comment, Dictionary<int, List<Comment>> repliesByParent)
    {
        var imagePaths = new List<string>();
        foreach (var img in comment.Images)
        {
            if (img?.StorageItem == null)
            {
                continue;
            }
            var url = await r2Storage.SignUrl(img.StorageItem.StoragePath);
            imagePaths.Add(url);
        }

        var filePaths = new List<string>();
        foreach (var file in comment.Files)
        {
            if (file?.StorageItem == null)
            {
                continue;
            }
            var url = await r2Storage.SignUrl(file.StorageItem.StoragePath);
            filePaths.Add(url);
        }

        var model = new CommentItemModel
        {
            Id = comment.Id,
            CreatedAt = comment.CreatedAt,
            Text = comment.Text,
            User = new UserItemModel
            {
                UserName = comment.User.UserName,
                Email = comment.User.Email
            },
            ImagePaths = imagePaths,
            FilePaths = filePaths,
            ReplyComments = new List<CommentItemModel>()
        };

        if (repliesByParent.TryGetValue(comment.Id, out var replies))
        {
            foreach (var reply in replies)
            {
                var mappedReply = await MapCommentAsync(reply, repliesByParent);
                model.ReplyComments.Add(mappedReply);
            }
        }

        return model;
    }
}