using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class InMemoryWorkloadProvider : IInMemoryWorkloadService
{
    public IQueryable<WorkPaper>? WorkPapers { get; }

    public InMemoryWorkloadProvider()
    {

    }
}
