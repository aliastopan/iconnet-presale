using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IInMemoryPersistenceService
{
    IQueryable<WorkPaper>? InProgressWorkPapers { get; }

    bool InsertInProgress(WorkPaper workPaper);
    int InsertRangeInProgress(IEnumerable<WorkPaper> workPapers);
    int InsertOverwriteInProgress(IEnumerable<WorkPaper> workPapers);
    int InsertOverwriteInProgress(IEnumerable<WorkPaper> workPapers, Func<WorkPaper, bool> filter);

    WorkPaper? Get(string idPermohonan);

    bool DeleteInProgress(WorkPaper workPaper);
}
