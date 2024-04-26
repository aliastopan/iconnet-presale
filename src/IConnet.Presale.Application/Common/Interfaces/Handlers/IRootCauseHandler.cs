using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IRootCauseHandler
{
    Result<ICollection<RootCause>> TryGetRootCauses();
    Task AddRootCauseAsync(int order, string cause, string classification);
    Task<Result> UpdateRootCauseAsync(Guid rootCauseId, string cause, string classification);
    Task<Result> ToggleOptionsAsync(Guid rootCauseId, bool isDeleted, bool isOnVerification);
}
