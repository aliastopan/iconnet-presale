
namespace IConnet.Presale.WebApp.Services;

public class SynchronizationService : BackgroundService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;
    private readonly ISqlSynchronizationManager _sqlSynchronizationManager;

    public SynchronizationService(IWorkloadSynchronizationManager workloadSynchronizationManager,
        ISqlSynchronizationManager sqlSynchronizationManager)
    {
        _workloadSynchronizationManager = workloadSynchronizationManager;
        _sqlSynchronizationManager = sqlSynchronizationManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _workloadSynchronizationManager.ProcessSynchronizeTasks();
            await _sqlSynchronizationManager.ProcessSqlPushTasks();

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
