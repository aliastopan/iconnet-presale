using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IRootCauseHandler
{
    Result<ICollection<RootCause>> TryGetRootCauses();
    Task AddRootCauseAsync(int order, string cause);
    Task<Result> UpdateRootCauseAsync(Guid rootCauseId, string cause);
    Task<Result> ToggleSoftDeletionAsync(Guid rootCauseId, bool isDeleted);
}
