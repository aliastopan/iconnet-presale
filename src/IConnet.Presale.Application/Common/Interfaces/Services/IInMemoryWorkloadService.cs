using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IInMemoryWorkloadService
{
    IQueryable<WorkPaper>? WorkPapers { get; }

    void Insert(WorkPaper workPaper);
    void InsertRange(IEnumerable<WorkPaper> workPapers);
}
