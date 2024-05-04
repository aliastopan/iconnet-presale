using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IPresaleDataBoundaryManager
{
    Task<IQueryable<WorkPaper>> GetPresaleDataFromCurrentMonthAsync();
    Task<IQueryable<WorkPaper>?> GetPresaleDataFromCurrentWeekAsync();
    IQueryable<WorkPaper> GetPresaleDataFromToday(IQueryable<WorkPaper> presaleData);

    Task<IQueryable<WorkPaper>?> GetUpperBoundaryPresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax);
    IQueryable<WorkPaper>? GetUpperBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTimeMin, DateTime dateTimeMax);
    IQueryable<WorkPaper>? GetMiddleBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTimeMin, DateTime dateTimeMax);
    IQueryable<WorkPaper>? GetLowerBoundaryPresaleData(IQueryable<WorkPaper> presaleData, DateTime dateTime);

    Task<IQueryable<WorkPaper>?> GetBoundaryPointPresaleDataAsync(DateTime dateTime);
    Task<IQueryable<WorkPaper>?> GetBoundaryRangePresaleDataAsync(DateTime dateTimeMin, DateTime dateTimeMax);
    Task<IQueryable<WorkPaper>?> GetBoundaryChunkPresaleDataAsync(int offset);
}
