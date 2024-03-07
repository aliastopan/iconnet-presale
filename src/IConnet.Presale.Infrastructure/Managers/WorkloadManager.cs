using System.Text.Json;
using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class WorkloadManager : IWorkloadManager
{
    private readonly WorkPaperFactory _workloadFactory;
    private readonly IRedisService _redisService;

    public WorkloadManager(WorkPaperFactory workloadFactory,
        IRedisService cacheService)
    {
        _workloadFactory = workloadFactory;
        _redisService = cacheService;
    }

    public async Task<int> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels)
    {
        int workloadCount = 0;

        foreach (var importModel in importModels)
        {
            var workPaper = _workloadFactory.CreateWorkPaper(importModel);

            var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
            var key = workPaper.ApprovalOpportunity.IdPermohonan;

            var isKeyExists = await _redisService.IsKeyExistsAsync(key);
            if (!isKeyExists)
            {
                await _redisService.SetValueAsync(key, jsonWorkPaper);
                workloadCount++;
            }
        }

        return workloadCount;
    }

    public async Task<IQueryable<WorkPaper>> GetWorkloadAsync(WorkloadFilter filter = WorkloadFilter.All)
    {
        List<WorkPaper> workPapers = [];
        List<string?> jsonWorkPapers = await _redisService.GetAllValuesAsync();

        foreach (var json in jsonWorkPapers)
        {
            if (json is null)
            {
                continue;
            }

            var workPaper = JsonSerializer.Deserialize<WorkPaper>(json)!;

            switch (filter)
            {
                case WorkloadFilter.OnlyImportVerified:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Verified)
                    {
                        continue;
                    }
                    break;
                case WorkloadFilter.OnlyImportUnverified:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Unverified)
                    {
                        continue;
                    }
                    break;
                case WorkloadFilter.OnlyImportArchived:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Invalid)
                    {
                        continue;
                    }
                    break;
                case WorkloadFilter.OnlyValidating:
                    if (workPaper.ApprovalOpportunity.StatusImport != ImportStatus.Verified
                        && workPaper.SignatureHelpdeskInCharge.IsEmptySignature())
                    {
                        continue;
                    }
                    break;
            }

            workPapers.Add(workPaper);
        }

        return workPapers.AsQueryable();
    }

    public async Task<bool> UpdateWorkloadAsync(WorkPaper workPaper)
    {
        var cacheKey = workPaper.ApprovalOpportunity.IdPermohonan;
        var isWorkPaperExist = await _redisService.IsKeyExistsAsync(cacheKey);
        if (!isWorkPaperExist)
        {
            return false;
        }

        var jsonWorkPaper = JsonSerializer.Serialize<WorkPaper>(workPaper);
        await _redisService.SetValueAsync(cacheKey, jsonWorkPaper);

        return true;
    }

    public async Task<bool> DeleteWorkloadAsync(WorkPaper workPaper)
    {
        var cacheKey = workPaper.ApprovalOpportunity.IdPermohonan;
        var isWorkPaperExist = await _redisService.IsKeyExistsAsync(cacheKey);
        if (!isWorkPaperExist)
        {
            return false;
        }

        return await _redisService.DeleteValueAsync(cacheKey);
    }

    public Task<int> SynchronizeRedisToInMemoryAsync()
    {
        throw new NotImplementedException();
    }
}
