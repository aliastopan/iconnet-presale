using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class WorkloadManager : IWorkloadManager
{
    private readonly IDateTimeService _dateTimeService;

    public WorkloadManager(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public Task CreateWorkPaper(IApprovalOpportunityModel ImportModel)
    {
       return Task.CompletedTask;
    }
}
