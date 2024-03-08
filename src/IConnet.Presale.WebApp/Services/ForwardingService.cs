
namespace IConnet.Presale.WebApp.Services;

public class ForwardingService : BackgroundService
{
    private readonly IWorkloadForwardingManager _workloadForwardingManager;

    public ForwardingService(IWorkloadForwardingManager workloadForwardingManager)
    {
        _workloadForwardingManager = workloadForwardingManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _workloadForwardingManager.ProcessForwardingTasks();
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
