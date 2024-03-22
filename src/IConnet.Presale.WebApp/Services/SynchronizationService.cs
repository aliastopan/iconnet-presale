
namespace IConnet.Presale.WebApp.Services;

public class SynchronizationService : BackgroundService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;

    public SynchronizationService(IWorkloadSynchronizationManager workloadSynchronizationManager)
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
