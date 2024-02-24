using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadManager
{
    Task<int> InsertWorkloadAsync(List<IApprovalOpportunityModel> importModels);
    Task<IQueryable<WorkPaper>> FetchWorkloadAsync(CacheFetchMode cacheFetchMode = CacheFetchMode.All);
    Task<bool> UpdateWorkloadAsync(WorkPaper workPaper);
    Task<bool> DeleteWorkloadAsync(WorkPaper workPaper);
}
