using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadManager
{
    Task<(int, HashSet<string>)> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels);
    Task<IQueryable<WorkPaper>> GetWorkloadAsync(PresaleDataFilter filter = PresaleDataFilter.All);
    Task UpdateWorkloadAsync(WorkPaper workPaper);
    Task DeleteWorkloadAsync(WorkPaper workPaper);
    Task ReinstateWorkloadAsync(WorkPaper workPaper);

    Task<WorkPaper?> SearchWorkPaperAsync(string idPermohonan);
    Task<IQueryable<WorkPaper>> GetArchivedPresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax);
}
