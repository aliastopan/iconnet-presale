using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IDashboardManager
{
    Task<IQueryable<WorkPaper>> GetPresaleDataFromCurrentMonthAsync();
}
