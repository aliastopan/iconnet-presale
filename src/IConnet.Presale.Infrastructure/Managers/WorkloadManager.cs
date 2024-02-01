using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class WorkloadManager : IWorkloadManager
{
    private readonly WorkloadFactory _workloadFactory;
    private readonly ICacheService _cacheService;

    public WorkloadManager(WorkloadFactory workloadFactory,
        ICacheService cacheService)
    {
        _workloadFactory = workloadFactory;
        _cacheService = cacheService;
    }

    public async Task<int> CacheWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        int workloadCount = 0;

        foreach (var importModel in importModels)
        {
            var workPaper = _workloadFactory.CreateWorkPaper(importModel);

            var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
            var key = workPaper.ApprovalOpportunity.IdPermohonan;

            var isKeyExists = await _cacheService.IsKeyExistsAsync(key);
            if (!isKeyExists)
            {
                await _cacheService.SetCacheValueAsync(key, jsonWorkPaper);
                workloadCount++;
            }
        }

        return workloadCount;
    }

    public async Task<List<WorkPaper>> FetchWorkloadAsync(CacheFetchMode cacheFetchMode = CacheFetchMode.All)
    {
        List<WorkPaper> workPapers = [];
        List<string?> jsonWorkPapers = await _cacheService.GetAllCacheValuesAsync();

        foreach (var json in jsonWorkPapers)
        {
            if (json is null)
            {
                continue;
            }

            var workPaper = JsonSerializer.Deserialize<WorkPaper>(json)!;

            switch (cacheFetchMode)
            {
                case CacheFetchMode.OnlyImportVerified:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Verified)
                    {
                        continue;
                    }
                    break;
                case CacheFetchMode.OnlyImportUnverified:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Unverified)
                    {
                        continue;
                    }
                    break;
                case CacheFetchMode.OnlyStaged:
                    if (workPaper.ApprovalOpportunity.StatusImport == ImportStatus.Unverified
                        || workPaper.HelpdeskInCharge.IsEmptySignature())
                    {

                        continue;
                    }
                    break;
            }

            workPapers.Add(workPaper);
        }

        return workPapers;
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
