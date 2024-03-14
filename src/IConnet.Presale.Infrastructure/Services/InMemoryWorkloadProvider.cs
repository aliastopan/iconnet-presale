using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class InMemoryWorkloadProvider : IInMemoryWorkloadService
{
    private readonly List<WorkPaper> _workPapers = [];

    public IQueryable<WorkPaper>? WorkPapers => _workPapers.AsQueryable();

    public InMemoryWorkloadProvider()
    {

    }

    public bool Insert(WorkPaper workPaper)
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

    public int InsertRange(IEnumerable<WorkPaper> workPapers)
    {
        _workPapers.AddRange(workPapers);

        return _workPapers.Count;
    }

    public int InsertOverwrite(IEnumerable<WorkPaper> workPapers)
    {
        _workPapers.Clear();
        _workPapers.AddRange(workPapers);

        return _workPapers.Count;
    }

    public int InsertOverwrite(IEnumerable<WorkPaper> workPapers, Func<WorkPaper, bool> filter)
    {
        _workPapers.Clear();
        _workPapers.AddRange(workPapers.Where(filter));

        return _workPapers.Count;
    }

    public WorkPaper? Get(string idPermohonan)
    {
        return _workPapers.FirstOrDefault(x => x.ApprovalOpportunity.IdPermohonan == idPermohonan);
    }

    public bool Delete(WorkPaper workPaper)
    {
        return _workPapers.Remove(workPaper);
    }
}
