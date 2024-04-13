using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.DirectApprovals.Commands.ToggleSoftDeletion;

public class ToggleDirectApprovalSoftDeletionCommand : IToggleDirectApprovalSoftDeletion, IRequest<Result>
{
    public ToggleDirectApprovalSoftDeletionCommand(Guid directApprovalId, bool isDeleted)
    {
        DirectApprovalId = directApprovalId;
        IsDeleted = isDeleted;
    }

    public Guid DirectApprovalId { get; set; }
    public bool IsDeleted { get; set; }
}
