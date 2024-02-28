using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class ParallelWorkloadManager : IWorkloadManager
{
    private readonly WorkPaperFactory _workloadFactory;
    private readonly ICacheService _cacheService;

    private readonly int _processorCount;
    private readonly ParallelOptions _parallelOptions;

    public ParallelWorkloadManager(WorkPaperFactory workloadFactory,
        ICacheService cacheService)
    {
        _workloadFactory = workloadFactory;
        _cacheService = cacheService;

        _processorCount = Environment.ProcessorCount;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _processorCount
        };
    }

    public async Task<int> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        int count = 0;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // check existing keys in a single batch operation
        var keysToCheck = importModels.Select(importModel => importModel.IdPermohonan).ToList();
        var existingKeys = await _cacheService.GetExistingKeysAsync(keysToCheck);

        var tasks = importModels
            .Where(importModel => !existingKeys.Contains(importModel.IdPermohonan))
            .Select(async importModel =>
            {
                var workPaper = _workloadFactory.CreateWorkPaper(importModel);
                var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
                var key = workPaper.ApprovalOpportunity.IdPermohonan;

                await _cacheService.SetValueAsync(key, jsonWorkPaper);
                Interlocked.Increment(ref count);
            })
            .ToList();

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        double seconds = stopwatch.ElapsedMilliseconds / 1000.0;
        LogSwitch.Debug($"Import execution took {seconds:F2} seconds.");

        return count;
    }

    public async Task<IQueryable<WorkPaper>> FetchWorkloadAsync(CacheFetchMode cacheFetchMode = CacheFetchMode.All)
    {
        var stopwatch = new Stopwatch();
        double seconds = stopwatch.ElapsedMilliseconds / 1000.0;

        stopwatch.Start();

        var jsonWorkPapers = await _cacheService.GetAllValuesAsync();
        var concurrentBag = new ConcurrentBag<string>();

        Parallel.ForEach(source: jsonWorkPapers, _parallelOptions, json =>
        {
            if (json != null && JsonWorkPaperProcessor.ShouldOnlyInclude(json, cacheFetchMode))
            {
                concurrentBag.Add(json);
            }
        });

        jsonWorkPapers = concurrentBag.ToList()!;

        stopwatch.Stop();
        seconds = stopwatch.ElapsedMilliseconds / 1000.0;
        LogSwitch.Debug($"Parallel filter workload execution took {seconds:F2} seconds.");

        stopwatch.Restart();

        var tasks = jsonWorkPapers.Select(json => Task.Run(() =>
        {
            try
            {
                if (json is null)
                {
                    throw new JsonException();
                }

                return JsonSerializer.Deserialize<WorkPaper>(json);
            }
            catch (JsonException exception)
            {
                Log.Fatal("Error deserializing JSON: {message}", exception.Message);
                return null;
            }
        }))
        .ToList();

        var workPapers = await Task.WhenAll(tasks);

        stopwatch.Stop();
        seconds = stopwatch.ElapsedMilliseconds / 1000.0;
        LogSwitch.Debug($"Parallel deserializing workload execution took {seconds:F2} seconds.");

        return workPapers.AsQueryable()!;
    }

    public async Task<bool> UpdateWorkloadAsync(WorkPaper workPaper)
    {
        var cacheKey = workPaper.ApprovalOpportunity.IdPermohonan;
        var isWorkPaperExist = await _cacheService.IsKeyExistsAsync(cacheKey);
        if (!isWorkPaperExist)
        {
            return false;
        }

        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
        await _cacheService.SetValueAsync(cacheKey, jsonWorkPaper);

        return true;
    }

    public async Task<bool> DeleteWorkloadAsync(WorkPaper workPaper)
    {
        var cacheKey = workPaper.ApprovalOpportunity.IdPermohonan;
        var isWorkPaperExist = await _cacheService.IsKeyExistsAsync(cacheKey);
        if (!isWorkPaperExist)
        {
            return false;
        }

        return await _cacheService.DeleteValueAsync(cacheKey);
    }
}
