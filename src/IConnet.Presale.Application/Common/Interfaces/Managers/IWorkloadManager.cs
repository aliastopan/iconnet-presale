using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadManager
{
    Task CreateWorkPaper(IApprovalOpportunityModel ApprovalOpportunityModel);
}
