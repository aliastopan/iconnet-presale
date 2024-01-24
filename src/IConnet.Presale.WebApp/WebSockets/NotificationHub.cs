using Microsoft.AspNetCore.SignalR;

namespace IConnet.Presale.WebApp.WebSockets;

public sealed class NotificationHub : Hub
{
    // replace with OnConnectionStart
    public async Task BroadcastNotification(string message)
    {
        await Clients.All.SendAsync("Broadcast", message);
    }
}
