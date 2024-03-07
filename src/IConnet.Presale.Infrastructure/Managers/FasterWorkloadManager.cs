using System.Diagnostics;
using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class FasterWorkloadManager : IWorkloadManager
{
    private readonly IInMemoryWorkloadService _inMemoryWorkloadService;
    private readonly IRedisService _redisService;
    private readonly WorkPaperFactory _workloadFactory;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;
    private bool _isInitialized = false;

    public FasterWorkloadManager(IInMemoryWorkloadService inMemoryWorkloadService,
        IRedisService redisService,
        WorkPaperFactory workloadFactory)
    {
        _inMemoryWorkloadService = inMemoryWorkloadService;
        _redisService = redisService;
        _workloadFactory = workloadFactory;

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public async Task<int> SynchronizeRedisToInMemoryAsync()
    {
        if (_isInitialized)
        {
            return 0;
        }

        var stopwatch = new Stopwatch();
        double seconds;

        var jsonWorkPapers = await _redisService.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapers(jsonWorkPapers!, _parallelOptions);

        int insertCount = _inMemoryWorkloadService.InsertOverwrite(workPapers);

        stopwatch.Stop();
        seconds = stopwatch.ElapsedMilliseconds / 1000.0;
        LogSwitch.Debug("Synchronize execution took {0} seconds.", $"{seconds:F2}");

        _isInitialized = true;

        return insertCount;
    }

    public async Task<int> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        var stopwatch = Stopwatch.StartNew();

        int count = 0;
        HashSet<string> keysToCheck = [];
        HashSet<string> existingIds = [];
        HashSet<string> existingKeys = [];

        keysToCheck = importModels.Select(importModel => importModel.IdPermohonan).ToHashSet();

        // check against in-memory cache
        existingIds = _inMemoryWorkloadService.WorkPapers!
            .Where(x => keysToCheck.Contains(x.ApprovalOpportunity.IdPermohonan))
            .Select(x => x.ApprovalOpportunity.IdPermohonan)
            .ToHashSet();

        // check against redis cache
        existingKeys = await _redisService.GetExistingKeysAsync(keysToCheck);

        // combine both sets
        existingIds.IntersectWith(existingKeys);

        // LogSwitch.Debug("Existing in-memory ids: {0}", existingIds.Count);
        // LogSwitch.Debug("Existing redis keys: {0}", existingKeys.Count);
        // LogSwitch.Debug("Combined existing ids/keys: {0}", existingIds.Count);

        // string idsString = string.Join(", ", existingIds);
        // string keysString = string.Join(", ", existingKeys);

        // LogSwitch.Debug("in-memory ids: {0}", existingIds);
        // LogSwitch.Debug("Redis keys: {0}", keysString);

        var workPapers = importModels
            .Where(workPaper => !existingIds.Contains(workPaper.IdPermohonan))
            .Select(_workloadFactory.CreateWorkPaper);

        _inMemoryWorkloadService.InsertRange(workPapers);

        var tasks = importModels
            .Where(importModel => !existingIds.Contains(importModel.IdPermohonan))
            .Select(async importModel =>
            {
                var workPaper = _workloadFactory.CreateWorkPaper(importModel);
                var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
                var key = workPaper.ApprovalOpportunity.IdPermohonan;

                await _redisService.SetValueAsync(key, jsonWorkPaper);
                Interlocked.Increment(ref count);
            });

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
        LogSwitch.Debug("Import execution took {0} seconds.", $"{seconds:F2}");

        return count;
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

        LogSwitch.Debug("Get execution took {0} seconds.", $"{seconds:F2}");
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

    public async Task<bool> UpdateWorkloadAsync(WorkPaper workPaper)
    {
        await Task.CompletedTask;

        return true;
    }

    public async Task<bool> DeleteWorkloadAsync(WorkPaper workPaper)
    {
        await Task.CompletedTask;

        return true;
    }
}
