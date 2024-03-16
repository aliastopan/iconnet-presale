using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Handlers;

internal sealed class WorkPaperAggregateHandler : IWorkPaperAggregateHandler
{
    public Task<Result> TryInsertWorkPaper(IWorkPaperModel workPaperModel)
    {
        throw new NotImplementedException();
    }
}
