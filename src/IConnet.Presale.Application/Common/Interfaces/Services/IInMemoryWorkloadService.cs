using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IInMemoryWorkloadService
{
    IQueryable<WorkPaper>? WorkPapers { get; }
}
