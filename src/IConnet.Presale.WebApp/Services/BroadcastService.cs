using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using IConnet.Presale.WebApp.WebSockets;

namespace IConnet.Presale.WebApp.Services;

public sealed class BroadcastService : IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IHubContext<BroadcastHub> _hubContext;
    private readonly HubConnection _hubConnection;

    public BroadcastService(IConfiguration configuration,
        IHubContext<BroadcastHub> hubContext)
    {
        _configuration = configuration;
        _hubContext = hubContext;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_configuration["WebSocket:BaseAddress"]}/broadcast")
            .Build();
    }

    public HubConnection HubConnection => _hubConnection;

    public async Task StartConnectionAsync()
    {
        await _hubConnection.StartAsync();

        switch (_hubConnection.State)
        {
            case HubConnectionState.Connected:
                Log.Warning("WebSocket connection established.");
                break;

            default:
                Log.Fatal("Failed to establish WebSocket connection. Current state: {0}", _hubConnection.State);
                break;
        }
    }

    public async Task BroadcastMessageAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("Broadcast", message);
    }

    public void Subscribe(Func<string, Task> action)
    {
        HubConnection.On("Broadcast", async (string message) =>
        {
            await action(message);
        });
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
