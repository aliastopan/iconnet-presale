using System.Diagnostics;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class FasterWorkloadManager : IWorkloadManager
{
    private readonly IInMemoryWorkloadService _inMemoryWorkloadService;
    private readonly WorkPaperFactory _workloadFactory;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;

    public FasterWorkloadManager(IInMemoryWorkloadService inMemoryWorkloadService,
        WorkPaperFactory workloadFactory)
    {
        _inMemoryWorkloadService = inMemoryWorkloadService;
        _workloadFactory = workloadFactory;

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public async Task<int> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        var stopwatch = Stopwatch.StartNew();

        var existingIds = _inMemoryWorkloadService.WorkPapers?
            .Select(x => x.ApprovalOpportunity.IdPermohonan)
            .ToHashSet();

        var workPapers = importModels
            .Where(workPaper => !existingIds!.Contains(workPaper.IdPermohonan))
            .Select(_workloadFactory.CreateWorkPaper);

        _inMemoryWorkloadService.InsertRange(workPapers);

        stopwatch.Stop();
        double seconds = stopwatch.ElapsedMilliseconds / 1000.0;

        LogSwitch.Debug($"Import execution took {seconds:F2} seconds.");
        await Task.CompletedTask;

        return workPapers.Count();
    }

    public async Task<IQueryable<WorkPaper>> GetWorkloadAsync(WorkloadFilter filter = WorkloadFilter.All)
    {
        var stopwatch = Stopwatch.StartNew();

        IQueryable<WorkPaper> workPapers = filter switch
        {
            WorkloadFilter.OnlyImportUnverified => FilterOnly(WorkPaperLevel.ImportUnverified),
            WorkloadFilter.OnlyImportInvalid => FilterOnly(WorkPaperLevel.ImportInvalid),
            WorkloadFilter.OnlyImportArchived => FilterOnly(WorkPaperLevel.ImportArchived),
            WorkloadFilter.OnlyImportVerified => FilterOnly(WorkPaperLevel.ImportVerified),
            WorkloadFilter.OnlyValidating => FilterOnly(WorkPaperLevel.ImportVerified, WorkPaperLevel.Validating),
            WorkloadFilter.OnlyWaitingApproval => FilterOnly(WorkPaperLevel.WaitingApproval),
            WorkloadFilter.OnlyDoneProcessing => FilterOnly(WorkPaperLevel.DoneProcessing),
            _ => _inMemoryWorkloadService.WorkPapers!.AsQueryable()
        };

        stopwatch.Stop();
        double seconds = stopwatch.ElapsedMilliseconds / 1000.0;

        LogSwitch.Debug($"Get execution took {seconds:F2} seconds.");
        await Task.CompletedTask;

        return workPapers;

        IQueryable<WorkPaper> FilterOnly(params WorkPaperLevel[] levels)
        {
            if (levels.Length == 1)
            {
                return _inMemoryWorkloadService.WorkPapers!.Where(workPaper => workPaper.WorkPaperLevel == levels[0]);
            }
            else
            {
                return _inMemoryWorkloadService.WorkPapers!.Where(workPaper => levels.Any(level => workPaper.WorkPaperLevel == level));
            }
        }
    }

    public Task<bool> UpdateWorkloadAsync(WorkPaper workPaper)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteWorkloadAsync(WorkPaper workPaper)
    {
        throw new NotImplementedException();
    }
}
