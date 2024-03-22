
namespace IConnet.Presale.WebApp.Services;

public class ForwardingService : BackgroundService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;

    public ForwardingService(IWorkloadSynchronizationManager workloadSynchronizationManager)
    {
        _workloadSynchronizationManager = workloadSynchronizationManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _workloadSynchronizationManager.ProcessSynchronizeTasks();
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
