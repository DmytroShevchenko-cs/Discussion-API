namespace Discussion.Core.Infrastructure.Hubs;

using Constants.Hub;
using Microsoft.AspNetCore.SignalR;

public class CommentsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, CommentsHubConsts.CommentsGroup);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}