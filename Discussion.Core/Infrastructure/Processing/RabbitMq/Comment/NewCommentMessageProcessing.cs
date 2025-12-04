namespace Discussion.Core.Infrastructure.Processing.RabbitMq.Comment;

using Common.Helpers;
using Constants.Hub;
using Constants.Storage;
using Database;
using Database.Entities.Comment;
using Database.Entities.Storage;
using Database.Entities.User;
using DTO.Hub.Comments.NewComment;
using Hubs;
using Messages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NodaTime;
using Services.R2StorageService;

public class NewCommentMessageProcessing
{
    private readonly ILogger<NewCommentMessageProcessing> _logger;
    private readonly IHubContext<CommentsHub> _hubContext;
    private readonly BaseDbContext _dbContext;
    private readonly IR2StorageService _storageService;

    public NewCommentMessageProcessing(ILogger<NewCommentMessageProcessing> logger, IHubContext<CommentsHub> hubContext,
        BaseDbContext dbContext, IR2StorageService storageService)
    {
        _logger = logger;
        _hubContext = hubContext;
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task Invoke(CommentMessageDTO request)
    {
        try
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    IsDeleted = false
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            var commentImages = new List<CommentImage>();
            var imageUrls = new List<string>();

            foreach (var img in request.Images)
            {
                var resizedContent = await ResizeImageHelper.ResizeImageAsync(new MemoryStream(img.Content));

                var storageItemInternal = await _storageService.UploadFileAsync(
                    new MemoryStream(resizedContent),
                    img.FileName,
                    null,
                    StorageDirectories.Images
                );

                commentImages.Add(new CommentImage
                {
                    StorageItem = new StorageItem
                    {
                        BaseName = storageItemInternal.BaseName,
                        FileName = storageItemInternal.FileName,
                        ItemHash = storageItemInternal.ItemHash,
                        Extension = storageItemInternal.Extension,
                        FileSize = storageItemInternal.FileSize,
                        StoragePath = storageItemInternal.StoragePath
                    }
                });

                var signedUrl =
                    await _storageService.SignUrl(storageItemInternal.StoragePath);
                imageUrls.Add(signedUrl);
            }

            var commentFiles = new List<CommentFile>();
            var fileUrls = new List<string>();

            foreach (var att in request.Attachments)
            {
                var storageItemInternal = await _storageService.UploadFileAsync(
                    new MemoryStream(att.Content),
                    att.FileName,
                    null,
                    StorageDirectories.Files
                );

                commentFiles.Add(new CommentFile
                {
                    StorageItem = new StorageItem
                    {
                        BaseName = storageItemInternal.BaseName,
                        FileName = storageItemInternal.FileName,
                        ItemHash = storageItemInternal.ItemHash,
                        Extension = storageItemInternal.Extension,
                        FileSize = storageItemInternal.FileSize,
                        StoragePath = storageItemInternal.StoragePath
                    }
                });

                var signedUrl =
                    await _storageService.SignUrl(storageItemInternal.StoragePath);
                fileUrls.Add(signedUrl);
            }

            var comment = new Comment
            {
                CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                Text = request.Text,
                HeadCommentId = request.ParentId,
                UserId = user.Id,
                Images = commentImages,
                Files = commentFiles,
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
            
            var dto = new NewCommentDTO
            {
                HeadCommentId = comment.HeadCommentId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt.ToString(),
                User = new UserItemModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                },
                Images = imageUrls,
                Files = fileUrls
            };

            await _hubContext.Clients.Group(CommentsHubConsts.CommentsGroup)
                .SendAsync(CommentsHubConsts.MessageType.NewComment, dto);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}