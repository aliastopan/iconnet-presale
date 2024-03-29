using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IWorkPaperAggregateHandler
{
    Task<Result> TryInsertOrUpdateWorkPaperAsync(IWorkPaperModel workPaperModel);
}
