using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadManager
{
    WorkPaper CreateWorkPaper(IApprovalOpportunityModel ImportModel);
}
