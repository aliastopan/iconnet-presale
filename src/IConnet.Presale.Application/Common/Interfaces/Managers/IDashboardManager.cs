using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IDashboardManager
{
    Task<IQueryable<WorkPaper>> GetPresaleDataFromCurrentMonthAsync();
    IQueryable<WorkPaper> GetPresaleDataFromCurrentWeek(IQueryable<WorkPaper> presaleData);
    IQueryable<WorkPaper> GetPresaleDataFromCurrentDay(IQueryable<WorkPaper> presaleData);
}
