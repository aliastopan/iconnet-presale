using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IInMemoryPersistenceService
{
    IQueryable<WorkPaper>? WorkPapers { get; }

    bool Insert(WorkPaper workPaper);
    int InsertRange(IEnumerable<WorkPaper> workPapers);
    int InsertOverwrite(IEnumerable<WorkPaper> workPapers);
    int InsertOverwrite(IEnumerable<WorkPaper> workPapers, Func<WorkPaper, bool> filter);

    WorkPaper? Get(string idPermohonan);

    bool Delete(WorkPaper workPaper);
}
