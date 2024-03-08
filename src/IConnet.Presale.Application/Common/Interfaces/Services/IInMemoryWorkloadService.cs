using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IInMemoryWorkloadService
{
    IQueryable<WorkPaper>? WorkPapers { get; }

    bool Insert(WorkPaper workPaper);
    int InsertRange(IEnumerable<WorkPaper> workPapers);
    int InsertOverwrite(IEnumerable<WorkPaper> workPapers);

    bool Delete(WorkPaper workPaper);
}
