using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using IConnet.Presale.WebApp.WebSockets;

namespace IConnet.Presale.WebApp.Services;

public sealed class NotificationService : IAsyncDisposable
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly HubConnection _hubConnection;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("wss://localhost:7101/broadcast")
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

    public async Task BroadcastNotificationAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync("Broadcast", message);
    }

    public void OnNotification(Action<string> action)
    {
        HubConnection.On("Broadcast", (string message) =>
        {
            action(message);
            Log.Warning("Broadcast: {0}", message);
        });
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
