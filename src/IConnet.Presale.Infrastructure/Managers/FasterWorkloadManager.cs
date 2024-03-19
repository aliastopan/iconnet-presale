using System.Diagnostics;
using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Extensions;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class FasterWorkloadManager : IWorkloadManager, IWorkloadForwardingManager
{
    private const int PartitionSize = 100;

    private readonly IDateTimeService _dateTimeService;
    private readonly IInMemoryWorkloadService _inMemoryWorkloadService;
    private readonly IOnProgressWorkloadPersistenceService _onProgressPersistence;
    private readonly WorkPaperFactory _workloadFactory;

    private readonly Queue<(string id, Task task)> _cacheForwardingTasks;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;
    private bool _isInitialized = false;

    public FasterWorkloadManager(IDateTimeService dateTimeService,
        IInMemoryWorkloadService inMemoryWorkloadService,
        IOnProgressWorkloadPersistenceService onProgressPersistence,
        WorkPaperFactory workloadFactory)
    {
        _dateTimeService = dateTimeService;
        _inMemoryWorkloadService = inMemoryWorkloadService;
        _onProgressPersistence = onProgressPersistence;
        _workloadFactory = workloadFactory;

        _cacheForwardingTasks = new Queue<(string id, Task task)>();

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
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
        existingKeys = await _onProgressPersistence.GetExistingKeysAsync(keysToCheck);

        // combine both sets
        existingIds.IntersectWith(existingKeys);

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

                await _onProgressPersistence.SetValueAsync(key, jsonWorkPaper);
                Interlocked.Increment(ref count);
            });

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        // LogSwitch.Debug("Import execution took {0} ms", stopwatch.ElapsedMilliseconds);

        return count;
    }

    public async Task<IQueryable<WorkPaper>> GetWorkloadAsync(WorkloadFilter filter = WorkloadFilter.All)
    {
        var stopwatch = Stopwatch.StartNew();

        IQueryable<WorkPaper> workPapers;

        var partitions = SplitIntoPartitions(_inMemoryWorkloadService.WorkPapers!, PartitionSize);
        var tasks = partitions.Select(partition => FilterPartitionAsync(filter, partition));

        workPapers = (await Task.WhenAll(tasks)).SelectMany(partition => partition).AsQueryable();

        stopwatch.Stop();
        // LogSwitch.Debug("Get execution took {0} ms", stopwatch.ElapsedMilliseconds);

        return workPapers;
    }

    public async Task UpdateWorkloadAsync(WorkPaper workPaper)
    {
        workPaper.LastModified = _dateTimeService.DateTimeOffsetNow;

        var key = workPaper.ApprovalOpportunity.IdPermohonan;
        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);

        var task = _onProgressPersistence.SetValueAsync(key, jsonWorkPaper);

        EnqueueForwardingTask(operationId: key, task);

        bool isDoneProcessing = workPaper.IsDoneProcessing();
        bool isInvalidCrmData = workPaper.IsInvalidCrmData();

        if (isDoneProcessing || isInvalidCrmData)
        {
            await _onProgressPersistence.SetBackupValueAsync(key, jsonWorkPaper);
            Log.Information("Moving {key} to Backup DB.", key);

            await _onProgressPersistence.DeleteValueAsync(key);
            _inMemoryWorkloadService.Delete(workPaper);
            Log.Information("Deleting {key} real-time access", key);
        }
    }

    public async Task DeleteWorkloadAsync(WorkPaper workPaper)
    {
        var key = workPaper.ApprovalOpportunity.IdPermohonan;

        var task = _onProgressPersistence.DeleteValueAsync(key);

        _inMemoryWorkloadService.Delete(workPaper);
        EnqueueForwardingTask(operationId: key, task);

        await Task.CompletedTask;
    }

    public async Task<WorkPaper?> SearchWorkPaper(string idPermohonan)
    {
        var workPaper = _inMemoryWorkloadService.Get(idPermohonan);

        if(workPaper is not null)
        {
            return workPaper;
        }

        var isKeyExist = await _onProgressPersistence.IsKeyExistsAsync(idPermohonan);

        if (!isKeyExist)
        {
            return null;
        }

        var jsonWorkPaper = await _onProgressPersistence.GetValueAsync(idPermohonan);

        return JsonWorkPaperProcessor.DeserializeJsonWorkPaper(jsonWorkPaper!);
    }

    private static async Task<IQueryable<WorkPaper>> FilterPartitionAsync(WorkloadFilter filter, IEnumerable<WorkPaper> partition)
    {
        return await Task.Run(() => FilterWorkload(filter, partition.AsQueryable()));
    }

    private static IQueryable<WorkPaper> FilterWorkload(WorkloadFilter filter, IQueryable<WorkPaper> workPapers)
    {
        return filter switch
        {
            WorkloadFilter.OnlyImportUnverified => FilterOnly(WorkPaperLevel.ImportUnverified),
            WorkloadFilter.OnlyImportInvalid => FilterOnly(WorkPaperLevel.ImportInvalid),
            WorkloadFilter.OnlyImportArchived => FilterOnly(WorkPaperLevel.ImportArchived),
            WorkloadFilter.OnlyImportVerified => FilterOnly(WorkPaperLevel.ImportVerified),
            WorkloadFilter.OnlyValidating => FilterOnly(WorkPaperLevel.ImportVerified, WorkPaperLevel.Validating),
            WorkloadFilter.OnlyWaitingApproval => FilterOnly(WorkPaperLevel.WaitingApproval),
            WorkloadFilter.OnlyDoneProcessing => FilterOnly(WorkPaperLevel.DoneProcessing),
            _ => workPapers
        };

        IQueryable<WorkPaper> FilterOnly(params WorkPaperLevel[] levels)
        {
            if (levels.Length == 1)
            {
                return workPapers.Where(workPaper => workPaper.WorkPaperLevel == levels[0]);
            }
            else
            {
                return workPapers.Where(workPaper => levels.Any(level => workPaper.WorkPaperLevel == level));
            }
        }
    }

    private static List<IEnumerable<WorkPaper>> SplitIntoPartitions(IEnumerable<WorkPaper> source, int partitionSize)
    {
        return source
            .Select((workPaper, index) => new
            {
                WorkPaper = workPaper,
                Index = index
            })
            .GroupBy(partition => partition.Index / partitionSize)
            .Select(group => group.Select(x => x.WorkPaper))
            .ToList();
    }

    public void EnqueueForwardingTask(string operationId, Task task)
    {
        lock (_cacheForwardingTasks)
        {
            _cacheForwardingTasks.Enqueue((operationId, task));
        }
    }

    public async Task ProcessForwardingTasks()
    {
        while (true)
        {
            (string id, Task task) cacheForwardingTask;

            lock (_cacheForwardingTasks)
            {
                if (_cacheForwardingTasks.Count == 0)
                {
                    break;
                }

                cacheForwardingTask = _cacheForwardingTasks.Dequeue();
            }

            try
            {
                await cacheForwardingTask.task;
            }
            catch (Exception exception)
            {
                Log.Fatal("Error executing task for operation {0}: {1}", cacheForwardingTask.id, exception.Message);
            }
        }
    }

    public async Task<int> ForwardRedisToInMemoryAsync()
    {
        if (_isInitialized)
        {
            return 0;
        }

        var stopwatch = Stopwatch.StartNew();

        var jsonWorkPapers = await _onProgressPersistence.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapers(jsonWorkPapers!, _parallelOptions);

        // int insertCount = _inMemoryWorkloadService.InsertOverwrite(workPapers);
        int insertCount = _inMemoryWorkloadService.InsertOverwrite(workPapers, excludeDoneProcessing);

        stopwatch.Stop();
        // LogSwitch.Debug("Forward execution took {0} ms", stopwatch.ElapsedMilliseconds);

        _isInitialized = true;

        return insertCount;

        static bool excludeDoneProcessing(WorkPaper workPaper)
        {
            return workPaper.WorkPaperLevel != WorkPaperLevel.DoneProcessing;
        }
    }
}
