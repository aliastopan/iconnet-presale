using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using IConnet.Presale.WebApp.WebSockets;

namespace IConnet.Presale.WebApp.Services;

public sealed class RealTimeService : IDisposable
{
    private readonly IHubContext<UpdateHub> _hubContext;
    private readonly HubConnection _hubConnection;

    public RealTimeService(IHubContext<UpdateHub> hubContext)
    {
        _hubContext = hubContext;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("wss://localhost:7101/update")
            .Build();
    }

    public HubConnection HubConnection => _hubConnection;

    public async Task StartConnectionAsync()
    {
        await _hubConnection.StartAsync();

        if (_hubConnection.State != HubConnectionState.Connected)
        {
            Log.Fatal("Failed to establish WebSocket connection. Current state: {0}", _hubConnection.State);
        }

        Log.Warning("WebSocket connection established.");
    }

    public async Task NotifyUpdateAsync(string id)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveUpdate", $"Key {id} has been updated");
    }

    public async Task OnUpdate(Action<string> action)
    {
        HubConnection.On("ReceiveUpdate", (string message) =>
        {
            if (message.Contains("redis"))
            {
                action(message);
            }

            return message;
        });

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _hubConnection.DisposeAsync().GetAwaiter().GetResult();
    }
}
