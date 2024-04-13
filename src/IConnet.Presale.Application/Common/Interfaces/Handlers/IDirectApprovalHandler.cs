using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IDirectApprovalHandler
{
    Result<ICollection<DirectApproval>> TryGetDirectApprovals();
    Task AddDirectApprovalAsync(int order, string description);
    Task<Result> ToggleSoftDeletionAsync(Guid directApprovalId, bool isDeleted);
}
