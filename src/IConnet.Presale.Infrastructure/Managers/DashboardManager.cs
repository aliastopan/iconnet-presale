using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class DashboardManager : PresaleDataOperationBase, IDashboardManager
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IInProgressPersistenceService _inProgressPersistenceService;
    private readonly IDoneProcessingPersistenceService _doneProcessingPersistenceService;

    public DashboardManager(IDateTimeService dateTimeService,
        IInProgressPersistenceService inProgressPersistenceService,
        IDoneProcessingPersistenceService doneProcessingPersistenceService)
    {
        _dateTimeService = dateTimeService;
        _inProgressPersistenceService = inProgressPersistenceService;
        _doneProcessingPersistenceService = doneProcessingPersistenceService;
    }

    public async Task<IQueryable<WorkPaper>> GetPresaleDataFromCurrentMonthAsync()
    {
        Task<List<string?>>[] getJsonWorkPapers =
        [
            _inProgressPersistenceService.GetAllValuesAsync(),
            _doneProcessingPersistenceService.GetAllValuesAsync()
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

    public IQueryable<WorkPaper> GetPresaleDataFromCurrentWeek(IQueryable<WorkPaper> presaleData)
    {
        var currentWeek = _dateTimeService.GetCurrentWeekOfYear();

        return presaleData.Where(workPaper => _dateTimeService.GetWeekOfYear(workPaper.ApprovalOpportunity.TglPermohonan) == currentWeek);
    }

    public IQueryable<WorkPaper> GetPresaleDataFromToday(IQueryable<WorkPaper> presaleData)
    {
        var currentDay = _dateTimeService.DateTimeOffsetNow.DayOfYear;

        return presaleData.Where(workPaper => workPaper.ApprovalOpportunity.TglPermohonan.DayOfYear == currentDay);
    }
}
