using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IDirectApprovalHandler
{
    Result<ICollection<DirectApproval>> TryGetDirectApprovals();
}
