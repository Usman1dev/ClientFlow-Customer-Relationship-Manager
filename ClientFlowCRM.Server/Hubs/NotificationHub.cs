using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ClientFlowCRM.Server.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    /// <summary>
    /// Sends a notification to a specific user by their connection group.
    /// </summary>
    public async Task SendNotificationToUser(string userId, string title, string message, string type)
    {
        await Clients.Group(userId).SendAsync("ReceiveNotification", title, message, type);
    }

    /// <summary>
    /// Broadcasts an announcement to all connected clients.
    /// </summary>
    public async Task SendAnnouncement(string title, string message)
    {
        await Clients.All.SendAsync("ReceiveAnnouncement", title, message);
    }

    /// <summary>
    /// Sends a general notification to all connected clients.
    /// </summary>
    public async Task BroadcastNotification(string title, string message, string type)
    {
        await Clients.All.SendAsync("ReceiveNotification", title, message, type);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
