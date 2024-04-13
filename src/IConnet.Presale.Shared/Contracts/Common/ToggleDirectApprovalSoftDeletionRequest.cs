namespace IConnet.Presale.Shared.Contracts.Common;

public record ToggleDirectApprovalSoftDeletionRequest(Guid DirectApprovalId, bool IsDeleted) : IToggleDirectApprovalSoftDeletion;
