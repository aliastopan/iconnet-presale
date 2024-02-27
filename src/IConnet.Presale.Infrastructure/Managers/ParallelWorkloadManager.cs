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

                await _cacheService.SetCacheValueAsync(key, jsonWorkPaper);
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
        await Task.CompletedTask;
        throw new NotImplementedException();
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
        await _cacheService.SetCacheValueAsync(cacheKey, jsonWorkPaper);

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

        return await _cacheService.DeleteCacheValueAsync(cacheKey);
    }
}
