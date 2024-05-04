namespace IConnet.Presale.WebApp.Services;

public class CommonDuplicateCollectorService : BackgroundService
{
    private readonly CommonDuplicateService _commonDuplicateService;
    private readonly IPresaleDataBoundaryManager _presaleDataBoundaryManager;
    private readonly PeriodicTimer _periodicTimer;

    public CommonDuplicateCollectorService(CommonDuplicateService commonDuplicateService,
        IPresaleDataBoundaryManager presaleDataBoundaryManager)
    {
        _commonDuplicateService = commonDuplicateService;
        _presaleDataBoundaryManager = presaleDataBoundaryManager;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromDays(3));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            IQueryable<WorkPaper>? presaleData = await _presaleDataBoundaryManager.GetBoundaryChunkPresaleDataAsync(offset: 31);

            _commonDuplicateService.SetCommonDuplicates(presaleData);

            Log.Information("Collecting potential duplicate {0}", _commonDuplicateService.PotentialDuplicates.Count);
        }
    }
}
