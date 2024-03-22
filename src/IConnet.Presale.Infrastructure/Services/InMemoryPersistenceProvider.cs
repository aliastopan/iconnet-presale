using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class InMemoryPersistenceProvider : IInMemoryPersistenceService
{
    private readonly List<WorkPaper> _workPapers = [];

    public IQueryable<WorkPaper>? InProgressWorkPapers => _workPapers.AsQueryable();

    public InMemoryPersistenceProvider()
    {

    }

    public bool InsertInProgress(WorkPaper workPaper)
    {
        try
        {
            _workPapers.Add(workPaper);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public int InsertRangeInProgress(IEnumerable<WorkPaper> workPapers)
    {
        _workPapers.AddRange(workPapers);

        return _workPapers.Count;
    }

    public int InsertOverwriteInProgress(IEnumerable<WorkPaper> workPapers)
    {
        _workPapers.Clear();
        _workPapers.AddRange(workPapers);

        return _workPapers.Count;
    }

    public int InsertOverwriteInProgress(IEnumerable<WorkPaper> workPapers, Func<WorkPaper, bool> filter)
    {
        _workPapers.Clear();
        _workPapers.AddRange(workPapers.Where(filter));

        return _workPapers.Count;
    }

    public WorkPaper? Get(string idPermohonan)
    {
        return _workPapers.FirstOrDefault(x => x.ApprovalOpportunity.IdPermohonan == idPermohonan);
    }

    public bool DeleteInProgress(WorkPaper workPaper)
    {
        return _workPapers.Remove(workPaper);
    }
}
