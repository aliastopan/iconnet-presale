using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class InMemoryWorkloadProvider : IInMemoryWorkloadService
{
    private readonly List<WorkPaper> _workPapers = [];

    public IQueryable<WorkPaper>? WorkPapers => _workPapers.AsQueryable();

    public InMemoryWorkloadProvider()
    {

    }

    public void Insert(WorkPaper workPaper)
    {
        _workPapers.Add(workPaper);
    }

    public void InsertRange(IEnumerable<WorkPaper> workPapers)
    {
        _workPapers.AddRange(workPapers);
    }
}
