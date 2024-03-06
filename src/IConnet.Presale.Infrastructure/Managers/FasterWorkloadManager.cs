using System.Diagnostics;
using IConnet.Presale.Domain.Aggregates.Presales;
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


    public Task<IQueryable<WorkPaper>> GetWorkloadAsync(WorkloadFilter filter = WorkloadFilter.All)
    {
        throw new NotImplementedException();
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
