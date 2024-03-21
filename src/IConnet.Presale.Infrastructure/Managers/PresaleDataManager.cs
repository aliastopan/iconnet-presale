using System.Diagnostics;
using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Extensions;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class PresaleDataManager : PresaleDataOperationBase, IWorkloadManager, IWorkloadForwardingManager
{
    private const int PartitionSize = 100;

    private readonly IDateTimeService _dateTimeService;
    private readonly IInMemoryPersistenceService _inMemoryPersistenceService;
    private readonly IOnProgressPersistenceService _onProgressPersistenceService;
    private readonly IDoneProcessingPersistenceService _doneProcessingPersistenceService;
    private readonly WorkPaperFactory _workPaperFactory;

    private readonly Queue<(string id, Task task)> _cacheForwardingTasks;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;
    private bool _isInitialized = false;

    public PresaleDataManager(IDateTimeService dateTimeService,
        IInMemoryPersistenceService inMemoryPersistenceService,
        IOnProgressPersistenceService onProgressPersistenceService,
        IDoneProcessingPersistenceService doneProcessingPersistenceService,
        WorkPaperFactory workloadFactory)
    {
        _dateTimeService = dateTimeService;
        _inMemoryPersistenceService = inMemoryPersistenceService;
        _onProgressPersistenceService = onProgressPersistenceService;
        _doneProcessingPersistenceService = doneProcessingPersistenceService;
        _workPaperFactory = workloadFactory;

        _cacheForwardingTasks = new Queue<(string id, Task task)>();

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public async Task<(int, HashSet<string>)> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        // var stopwatch = Stopwatch.StartNew();

        int count = 0;
        HashSet<string> keysToCheck = [];
        HashSet<string> existingIds = [];
        HashSet<string> existingKeys = [];
        HashSet<string> archivedKeys = [];

        keysToCheck = importModels.Select(importModel => importModel.IdPermohonan).ToHashSet();

        // check against in-memory cache
        existingIds = _inMemoryPersistenceService.WorkPapers!
            .Where(x => keysToCheck.Contains(x.ApprovalOpportunity.IdPermohonan))
            .Select(x => x.ApprovalOpportunity.IdPermohonan)
            .ToHashSet();

        // LogSwitch.Debug($"In-Memory Ids: {String.Join(", ", existingIds)}");

        // check against redis cache
        Task<HashSet<string>>[] getExistingKeysTask =
        [
            _onProgressPersistenceService.GetExistingKeysAsync(keysToCheck),
            _doneProcessingPersistenceService.GetExistingKeysAsync(keysToCheck)
        ];

        await Task.WhenAll(getExistingKeysTask);

        existingKeys = getExistingKeysTask[0].Result;
        archivedKeys = getExistingKeysTask[1].Result;

        // LogSwitch.Debug($"Existing Keys: {String.Join(", ", existingKeys)}");
        // LogSwitch.Debug($"Archived Keys: {String.Join(", ", archivedKeys)}");

        existingIds.UnionWith(existingKeys);
        existingIds.UnionWith(archivedKeys);

        // LogSwitch.Debug($"Existing Ids: {String.Join(", ", existingIds)}");

        var workPapers = importModels
            .Where(workPaper => !existingIds.Contains(workPaper.IdPermohonan))
            .Select(_workPaperFactory.CreateWorkPaper);

        _inMemoryPersistenceService.InsertRange(workPapers);

        var tasks = importModels
            .Where(importModel => !existingIds.Contains(importModel.IdPermohonan))
            .Select(async importModel =>
            {
                var workPaper = _workPaperFactory.CreateWorkPaper(importModel);
                var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
                var key = workPaper.ApprovalOpportunity.IdPermohonan;

                await _onProgressPersistenceService.SetValueAsync(key, jsonWorkPaper);
                Interlocked.Increment(ref count);
            });

        await Task.WhenAll(tasks);

        // stopwatch.Stop();
        // LogSwitch.Debug("Import execution took {0} ms", stopwatch.ElapsedMilliseconds);

        return (count, existingIds);
    }

    public async Task<IQueryable<WorkPaper>> GetWorkloadAsync(PresaleDataFilter filter = PresaleDataFilter.All)
    {
        // var stopwatch = Stopwatch.StartNew();

        IQueryable<WorkPaper> workPapers;

        var partitions = SplitIntoPartitions(_inMemoryPersistenceService.WorkPapers!, PartitionSize);
        var tasks = partitions.Select(partition => FilterPartitionAsync(filter, partition));

        workPapers = (await Task.WhenAll(tasks)).SelectMany(partition => partition).AsQueryable();

        // stopwatch.Stop();
        // LogSwitch.Debug("Get execution took {0} ms", stopwatch.ElapsedMilliseconds);

        return workPapers;
    }

    public async Task<IQueryable<WorkPaper>> GetArchivedWorkloadAsync(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        var jsonWorkPapers = await _doneProcessingPersistenceService.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers!, _parallelOptions,
            workPaper =>
            {
                return workPaper.ApprovalOpportunity.TglPermohonan >= dateTimeMin
                    && workPaper.ApprovalOpportunity.TglPermohonan <= dateTimeMax;
            });

        LogSwitch.Debug("Archived count: {0}", workPapers.Count());

        return workPapers.AsQueryable();
    }

    public async Task UpdateWorkloadAsync(WorkPaper workPaper)
    {
        workPaper.LastModified = _dateTimeService.DateTimeOffsetNow;

        var key = workPaper.ApprovalOpportunity.IdPermohonan;
        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);

        var task = _onProgressPersistenceService.SetValueAsync(key, jsonWorkPaper);

        EnqueueForwardingTask(operationId: key, task);

        bool isDoneProcessing = workPaper.IsDoneProcessing();
        bool isInvalidCrmData = workPaper.IsInvalidCrmData();

        if (isDoneProcessing || isInvalidCrmData)
        {
            await _doneProcessingPersistenceService.ArchiveValueAsync(key, jsonWorkPaper);
            Log.Information("Moving {key} to Backup DB.", key);

            await _onProgressPersistenceService.DeleteValueAsync(key);
            _inMemoryPersistenceService.Delete(workPaper);
            Log.Information("Deleting {key} real-time access", key);
        }
    }

    public async Task DeleteWorkloadAsync(WorkPaper workPaper)
    {
        var key = workPaper.ApprovalOpportunity.IdPermohonan;

        var task = _onProgressPersistenceService.DeleteValueAsync(key);

        _inMemoryPersistenceService.Delete(workPaper);
        EnqueueForwardingTask(operationId: key, task);

        await Task.CompletedTask;
    }

    public async Task<WorkPaper?> SearchWorkPaperAsync(string idPermohonan)
    {
        var workPaper = _inMemoryPersistenceService.Get(idPermohonan);
        if (workPaper is not null)
        {
            return workPaper;
        }

        var isKeyExist = await _onProgressPersistenceService.IsKeyExistsAsync(idPermohonan);
        if (isKeyExist)
        {
            var jsonWorkPaper = await _onProgressPersistenceService.GetValueAsync(idPermohonan);
            return JsonWorkPaperProcessor.DeserializeJsonWorkPaper(jsonWorkPaper!);
        }

        var isKeyArchived = await _doneProcessingPersistenceService.IsKeyExistsAsync(idPermohonan);
        if (isKeyArchived)
        {
            var jsonWorkPaper = await _doneProcessingPersistenceService.GetValueAsync(idPermohonan);
            return JsonWorkPaperProcessor.DeserializeJsonWorkPaper(jsonWorkPaper!);
        }

        return null;
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

        // var stopwatch = Stopwatch.StartNew();

        var jsonWorkPapers = await _onProgressPersistenceService.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapers(jsonWorkPapers!, _parallelOptions);

        // int insertCount = _inMemoryWorkloadService.InsertOverwrite(workPapers);
        int insertCount = _inMemoryPersistenceService.InsertOverwrite(workPapers, excludeDoneProcessing);

        // stopwatch.Stop();
        // LogSwitch.Debug("Forward execution took {0} ms", stopwatch.ElapsedMilliseconds);

        _isInitialized = true;

        return insertCount;

        static bool excludeDoneProcessing(WorkPaper workPaper)
        {
            return workPaper.WorkPaperLevel != WorkPaperLevel.DoneProcessing;
        }
    }
}
