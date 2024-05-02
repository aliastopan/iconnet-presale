using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class PresaleDataBoundaryManager : PresaleDataOperationBase, IPresaleDataBoundaryManager
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IInProgressPersistenceService _inProgressPersistenceService;
    private readonly IDoneProcessingPersistenceService _doneProcessingPersistenceService;

    public PresaleDataBoundaryManager(IDateTimeService dateTimeService,
        IInProgressPersistenceService inProgressPersistenceService,
        IDoneProcessingPersistenceService doneProcessingPersistenceService)
    {
        _dateTimeService = dateTimeService;
        _inProgressPersistenceService = inProgressPersistenceService;
        _doneProcessingPersistenceService = doneProcessingPersistenceService;
    }

    public async Task<IQueryable<WorkPaper>> GetPresaleDataFromCurrentMonthAsync()
    {
        long startUnixTime = _dateTimeService.GetUnixTime(_dateTimeService.DateTimeOffsetNow.DateTime.AddDays(-31));
        long endUnixTime = _dateTimeService.GetUnixTime(_dateTimeService.DateTimeOffsetNow.DateTime);

        Task<List<string?>>[] getJsonWorkPapers =
        [
            _inProgressPersistenceService.GetAllValuesAsync(),
            _doneProcessingPersistenceService.GetAllScoredValuesAsync(startUnixTime, endUnixTime)
        ];

        await Task.WhenAll(getJsonWorkPapers);

        var inProgressJsonWorkPapers = getJsonWorkPapers[0].Result;
        var doneProcessingJsonWorkPaper = getJsonWorkPapers[1].Result;

        Task<List<WorkPaper>>[] workPaperTasks =
        {
            Task.Run(() => ProcessJsonWorkPapers(inProgressJsonWorkPapers!)),
            Task.Run(() => ProcessJsonWorkPapers(doneProcessingJsonWorkPaper!))
        };

        await Task.WhenAll(workPaperTasks);

        List<WorkPaper> inProgressWorkPapers = workPaperTasks[0].Result;
        List<WorkPaper> doneProcessingWorkPapers = workPaperTasks[1].Result;

        return doneProcessingWorkPapers.Concat(inProgressWorkPapers).AsQueryable();

        // local function
        List<WorkPaper> ProcessJsonWorkPapers(List<string> jsonWorkPapers)
        {
            var currentMonth = _dateTimeService.DateTimeOffsetNow.Month;
            var currentYear = _dateTimeService.DateTimeOffsetNow.Year;

            return JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers, this.ParallelOptions,
                workPaper =>
                {
                    return workPaper.ApprovalOpportunity.TglPermohonan.Month == currentMonth
                        && workPaper.ApprovalOpportunity.TglPermohonan.Year == currentYear;
                });
        }
    }

    public async Task<IQueryable<WorkPaper>?> GetPresaleDataFromCurrentWeekAsync()
    {
        var today = _dateTimeService.DateTimeOffsetNow.DateTime;
        var firstDayOfWeek = today.AddDays(-(int)today.DayOfWeek);

        return await GetUpperBoundaryPresaleDataAsync(firstDayOfWeek, today);
    }

    public IQueryable<WorkPaper> GetPresaleDataFromToday(IQueryable<WorkPaper> presaleData)
    {
        var currentDay = _dateTimeService.DateTimeOffsetNow.DayOfYear;

        return presaleData.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.DayOfYear == currentDay);
    }

    public async Task<IQueryable<WorkPaper>?> GetUpperBoundaryPresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        long startUnixTime = _dateTimeService.GetUnixTime(dateTimeMin);
        long endUnixTime = _dateTimeService.GetUnixTime(dateTimeMax.AddDays(1));

        Task<List<string?>>[] getJsonWorkPapers =
        [
            _inProgressPersistenceService.GetAllValuesAsync(),
            _doneProcessingPersistenceService.GetAllScoredValuesAsync(startUnixTime, endUnixTime)
        ];

        await Task.WhenAll(getJsonWorkPapers);

        var inProgressJsonWorkPapers = getJsonWorkPapers[0].Result;
        var doneProcessingJsonWorkPaper = getJsonWorkPapers[1].Result;

        Task<List<WorkPaper>>[] workPaperTasks =
        {
            Task.Run(() => ProcessJsonWorkPapers(inProgressJsonWorkPapers!)),
            Task.Run(() => ProcessJsonWorkPapers(doneProcessingJsonWorkPaper!))
        };

        await Task.WhenAll(workPaperTasks);

        List<WorkPaper> inProgressWorkPapers = workPaperTasks[0].Result;
        List<WorkPaper> doneProcessingWorkPapers = workPaperTasks[1].Result;

        return doneProcessingWorkPapers.Concat(inProgressWorkPapers).AsQueryable();

        // local function
        List<WorkPaper> ProcessJsonWorkPapers(List<string> jsonWorkPapers)
        {
            return JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers, this.ParallelOptions);
        }
    }

    public IQueryable<WorkPaper>? GetUpperBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTimeMin, DateTime dateTimeMax)
    {
        return presaleData.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.Date >= dateTimeMin.Date
            && workPaper.ApprovalOpportunity.TglPermohonan.Date <= dateTimeMax.Date);
    }

    public IQueryable<WorkPaper>? GetMiddleBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTimeMin, DateTime dateTimeMax)
    {
        return presaleData.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.Date >= dateTimeMin.Date
            && workPaper.ApprovalOpportunity.TglPermohonan.Date <= dateTimeMax.Date);
    }

    public IQueryable<WorkPaper>? GetLowerBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTime)
    {
        return presaleData.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.Date == dateTime.Date);
    }

    public async Task<IQueryable<WorkPaper>?> GetBoundaryPointPresaleDataAsync(DateTime dateTime)
    {
        DateTime dateTimeMin = dateTime.AddDays(-1);
        DateTime dateTimeMax = dateTime.AddDays(1);

        IQueryable<WorkPaper>? boundaryRange = await GetBoundaryRangePresaleDataAsync(dateTimeMin, dateTimeMax);

        if (boundaryRange is null)
        {
            return null;
        }

        return boundaryRange.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.Date == dateTime.Date);
    }

    public async Task<IQueryable<WorkPaper>?> GetBoundaryRangePresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        int offset = _dateTimeService.GetFirstDayOfWeekOffset(dateTimeMin);

        long startUnixTime = _dateTimeService.GetUnixTime(dateTimeMin.AddDays(-offset));    // add first day of the week offset
        long endUnixTime = _dateTimeService.GetUnixTime(dateTimeMax.AddDays(1));            // add 1 day offset

        Task<List<string?>>[] getJsonWorkPapers =
        [
            _inProgressPersistenceService.GetAllValuesAsync(),
            _doneProcessingPersistenceService.GetAllScoredValuesAsync(startUnixTime, endUnixTime)
        ];

        await Task.WhenAll(getJsonWorkPapers);

        var inProgressJsonWorkPapers = getJsonWorkPapers[0].Result;
        var doneProcessingJsonWorkPaper = getJsonWorkPapers[1].Result;

        Task<List<WorkPaper>>[] workPaperTasks =
        {
            Task.Run(() => ProcessJsonWorkPapers(inProgressJsonWorkPapers!)),
            Task.Run(() => ProcessJsonWorkPapers(doneProcessingJsonWorkPaper!))
        };

        await Task.WhenAll(workPaperTasks);

        List<WorkPaper> inProgressWorkPapers = workPaperTasks[0].Result;
        List<WorkPaper> doneProcessingWorkPapers = workPaperTasks[1].Result;

        return doneProcessingWorkPapers.Concat(inProgressWorkPapers).AsQueryable();

        // local function
        List<WorkPaper> ProcessJsonWorkPapers(List<string> jsonWorkPapers)
        {
            return JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers, this.ParallelOptions);
        }
    }
}
