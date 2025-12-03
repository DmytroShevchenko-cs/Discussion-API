namespace Discussion.Core.Infrastructure.Hubs;

using System.Text.Json;
using Constants.Hub;
using DTO.Hub.Comments.NewComment;
using Microsoft.AspNetCore.SignalR;

public class CommentsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return Task.CompletedTask;
    }
    
    public async Task ConnectToDiscussion()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, CommentsHubConsts.CommentsGroup);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendComment(NewCommentDTO comment)
    {
        var message = JsonSerializer.Serialize(comment);
        
        await Clients.Group(CommentsHubConsts.CommentsGroup)
            .SendAsync(CommentsHubConsts.MessageType.NewComment, new 
            {
                User = Context.ConnectionId,
                Text = message,
                Timestamp = DateTime.UtcNow
            });
    }
    
    public async Task Echo(string message)
    {
        await Clients.All
            .SendAsync($"Receive message, {message} (echo from server)");
    }
}