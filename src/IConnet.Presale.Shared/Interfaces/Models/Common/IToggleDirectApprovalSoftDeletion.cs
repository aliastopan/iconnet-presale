namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IToggleDirectApprovalSoftDeletion
{
    public Guid DirectApprovalId { get; }
    public bool IsDeleted { get; }
}
