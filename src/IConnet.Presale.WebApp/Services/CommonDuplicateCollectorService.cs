namespace IConnet.Presale.WebApp.Services;

public class CommonDuplicateCollectorService : BackgroundService
{
    private readonly IPresaleDataBoundaryManager _presaleDataBoundaryManager;
    private readonly PeriodicTimer _periodicTimer;

    public CommonDuplicateCollectorService(IPresaleDataBoundaryManager presaleDataBoundaryManager)
    {
        _presaleDataBoundaryManager = presaleDataBoundaryManager;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            Log.Information("Collecting duplicate");
        }
    }
}
