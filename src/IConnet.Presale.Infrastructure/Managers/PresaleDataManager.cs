using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Extensions;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class PresaleDataManager : PresaleDataOperationBase,
    IWorkloadManager, IWorkloadSynchronizationManager, IRedisFormatter
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IInMemoryPersistenceService _inMemoryPersistenceService;
    private readonly IInProgressPersistenceService _inProgressPersistenceService;
    private readonly IDoneProcessingPersistenceService _doneProcessingPersistenceService;
    private readonly WorkPaperFactory _workPaperFactory;

    private readonly Queue<(string id, Task task)> _cacheSynchronizeTasks;

    private bool _isInitialized = false;

    public PresaleDataManager(IDateTimeService dateTimeService,
        IInMemoryPersistenceService inMemoryPersistenceService,
        IInProgressPersistenceService inProgressPersistenceService,
        IDoneProcessingPersistenceService doneProcessingPersistenceService,
        WorkPaperFactory workloadFactory)
    {
        _dateTimeService = dateTimeService;
        _inMemoryPersistenceService = inMemoryPersistenceService;
        _inProgressPersistenceService = inProgressPersistenceService;
        _doneProcessingPersistenceService = doneProcessingPersistenceService;
        _workPaperFactory = workloadFactory;

        _cacheSynchronizeTasks = new Queue<(string id, Task task)>();
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
        existingIds = _inMemoryPersistenceService.InProgressWorkPapers!
            .Where(x => keysToCheck.Contains(x.ApprovalOpportunity.IdPermohonan))
            .Select(x => x.ApprovalOpportunity.IdPermohonan)
            .ToHashSet();

        // LogSwitch.Debug($"In-Memory Ids: {String.Join(", ", existingIds)}");

        // check against redis cache
        Task<HashSet<string>>[] getExistingKeysTask =
        [
            _inProgressPersistenceService.GetExistingKeysAsync(keysToCheck),
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

        _inMemoryPersistenceService.InsertRangeInProgress(workPapers);

        var tasks = importModels
            .Where(importModel => !existingIds.Contains(importModel.IdPermohonan))
            .Select(async importModel =>
            {
                var workPaper = _workPaperFactory.CreateWorkPaper(importModel);
                var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
                var key = workPaper.ApprovalOpportunity.IdPermohonan;

                await _inProgressPersistenceService.SetValueAsync(key, jsonWorkPaper);
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

        var partitions = SplitIntoPartitions(_inMemoryPersistenceService.InProgressWorkPapers!);
        var tasks = partitions.Select(partition => FilterPartitionAsync(filter, partition));

        workPapers = (await Task.WhenAll(tasks)).SelectMany(partition => partition).AsQueryable();

        // stopwatch.Stop();
        // LogSwitch.Debug("Get execution took {0} ms", stopwatch.ElapsedMilliseconds);

        return workPapers;
    }

    public async Task<IQueryable<WorkPaper>> GetArchivedPresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        long startUnixTime = _dateTimeService.GetUnixTime(dateTimeMin);
        long endUnixTime = _dateTimeService.GetUnixTime(dateTimeMax);

        var jsonWorkPapers = await _doneProcessingPersistenceService.GetAllScoredValuesAsync(startUnixTime, endUnixTime);
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers!, ParallelOptions);

        return workPapers.AsQueryable();
    }

    public async Task UpdateWorkloadAsync(WorkPaper workPaper)
    {
        workPaper.LastModified = _dateTimeService.DateTimeOffsetNow;

        var key = workPaper.ApprovalOpportunity.IdPermohonan;
        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);

        var task = _inProgressPersistenceService.SetValueAsync(key, jsonWorkPaper);

        EnqueueSynchronizeTask(operationId: key, task);

        bool isDoneProcessing = workPaper.IsDoneProcessing();
        bool isInvalidCrmData = workPaper.IsInvalidCrmData();

        if (isDoneProcessing || isInvalidCrmData)
        {
            var unixTimestamp =  _dateTimeService.GetUnixTime(workPaper.ApprovalOpportunity.TglPermohonan);

            await _doneProcessingPersistenceService.ArchiveValueAsync(key, jsonWorkPaper, unixTimestamp);
            Log.Information("Moving {key} to Archive DB.", key);

            await _inProgressPersistenceService.DeleteValueAsync(key);
            _inMemoryPersistenceService.DeleteInProgress(workPaper);
            Log.Information("Deleting {key} real-time access", key);
        }
    }

    public async Task DeleteWorkloadAsync(WorkPaper workPaper)
    {
        var key = workPaper.ApprovalOpportunity.IdPermohonan;

        var task = _inProgressPersistenceService.DeleteValueAsync(key);

        _inMemoryPersistenceService.DeleteInProgress(workPaper);
        EnqueueSynchronizeTask(operationId: key, task);

        await Task.CompletedTask;
    }

    public async Task ReinstateWorkloadAsync(WorkPaper workPaper)
    {
        var key = workPaper.ApprovalOpportunity.IdPermohonan;
        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);

        Task[] tasks =
        [
            _inProgressPersistenceService.DeleteValueAsync(key),
            _doneProcessingPersistenceService.DeleteValueAsync(key)
        ];

        await Task.WhenAll(tasks);

        await _inProgressPersistenceService.SetValueAsync(key, jsonWorkPaper);
        _inMemoryPersistenceService.InsertInProgress(workPaper);
    }

    public async Task<WorkPaper?> SearchWorkPaperAsync(string idPermohonan)
    {
        var workPaper = _inMemoryPersistenceService.Get(idPermohonan);
        if (workPaper is not null)
        {
            return workPaper;
        }

        var isKeyExist = await _inProgressPersistenceService.IsKeyExistsAsync(idPermohonan);
        if (isKeyExist)
        {
            var jsonWorkPaper = await _inProgressPersistenceService.GetValueAsync(idPermohonan);
            return JsonWorkPaperProcessor.DeserializeJsonWorkPaper(jsonWorkPaper!);
        }

        var isKeyArchived = await _doneProcessingPersistenceService.IsKeyExistsAsync(idPermohonan);
        if (isKeyArchived)
        {
            var jsonWorkPaper = await _doneProcessingPersistenceService.GetScoredValueAsync(idPermohonan);
            return JsonWorkPaperProcessor.DeserializeJsonWorkPaper(jsonWorkPaper!);
        }

        return null;
    }

    public void EnqueueSynchronizeTask(string operationId, Task task)
    {
        lock (_cacheSynchronizeTasks)
        {
            _cacheSynchronizeTasks.Enqueue((operationId, task));
        }
    }

    public async Task ProcessSynchronizeTasks()
    {
        while (true)
        {
            (string id, Task task) cacheSynchronizeTask;

            lock (_cacheSynchronizeTasks)
            {
                if (_cacheSynchronizeTasks.Count == 0)
                {
                    break;
                }

                cacheSynchronizeTask = _cacheSynchronizeTasks.Dequeue();
            }

            try
            {
                await cacheSynchronizeTask.task;
            }
            catch (Exception exception)
            {
                Log.Fatal("Error executing task for operation {0}: {1}", cacheSynchronizeTask.id, exception.Message);
            }
        }
    }

    public async Task<int> PullRedisToInMemoryAsync()
    {
        if (_isInitialized)
        {
            return 0;
        }

        // var stopwatch = Stopwatch.StartNew();

        var jsonWorkPapers = await _inProgressPersistenceService.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers!, ParallelOptions);

        // int insertCount = _inMemoryWorkloadService.InsertOverwrite(workPapers);
        int insertCount = _inMemoryPersistenceService.InsertOverwriteInProgress(workPapers, excludeDoneProcessing);

        // stopwatch.Stop();
        // LogSwitch.Debug("Forward execution took {0} ms", stopwatch.ElapsedMilliseconds);

        _isInitialized = true;

        return insertCount;

        static bool excludeDoneProcessing(WorkPaper workPaper)
        {
            return workPaper.WorkPaperLevel != WorkPaperLevel.DoneProcessing;
        }
    }

    public async Task<int> ReformatArchiveAsync()
    {
        Log.Warning("Fetching Data.");

        var jsonWorkPapers = await _doneProcessingPersistenceService.GetAllValuesAsync();
        var workPapers = JsonWorkPaperProcessor.DeserializeJsonWorkPapersParallel(jsonWorkPapers!, ParallelOptions);

        Log.Warning("Data Fetched.");
        Log.Warning("Now Deleting.");

        int deleteCounter = 1;
        foreach (var workPaper in workPapers)
        {
            var key = workPaper.ApprovalOpportunity.IdPermohonan;
            await _doneProcessingPersistenceService.DeleteValueAsync(key);

            Log.Warning("Deleting: {0}/{1}", deleteCounter++, workPapers.Count);
        }

        Log.Warning("Delete Complete. Began Formatting");

        int counter = 1;

        foreach (var workPaper in workPapers)
        {
            var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
            var key = workPaper.ApprovalOpportunity.IdPermohonan;
            var unixTimestamp =  _dateTimeService.GetUnixTime(workPaper.ApprovalOpportunity.TglPermohonan);

            await _doneProcessingPersistenceService.ArchiveValueAsync(key, jsonWorkPaper, unixTimestamp);

            Log.Warning("Formatting: {0}/{1}", counter++, workPapers.Count);
        }

        Log.Warning("Formatting Complete.");

        return counter;
    }
}
