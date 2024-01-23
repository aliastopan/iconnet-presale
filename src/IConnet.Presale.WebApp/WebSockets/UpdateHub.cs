using Microsoft.AspNetCore.SignalR;

namespace IConnet.Presale.WebApp.WebSockets;

public sealed class UpdateHub : Hub
{
    public async Task SendUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveUpdate", message);
    }
}
