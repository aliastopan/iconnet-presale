using Microsoft.AspNetCore.SignalR;
using IConnet.Presale.WebApp.WebSockets;

namespace IConnet.Presale.WebApp.Services;

public sealed class RealTimeService
{
    private readonly IHubContext<UpdateHub> _hubContext;

    public RealTimeService(IHubContext<UpdateHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUpdateAsync(string id)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdate", $"Key {id} has been updated");
    }
}
