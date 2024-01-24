using Microsoft.AspNetCore.SignalR;

namespace IConnet.Presale.WebApp.WebSockets;

public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var message = $"Connection to {Context.ConnectionId} has been established.";
        await Clients.All.SendAsync("Broadcast", message);
    }
}
