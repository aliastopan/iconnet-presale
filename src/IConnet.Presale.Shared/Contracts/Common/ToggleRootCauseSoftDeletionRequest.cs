namespace IConnet.Presale.Shared.Contracts.Common;

public record ToggleRootCauseSoftDeletionRequest(Guid RootCauseId, bool IsDeleted) : IToggleRootCauseSoftDeletion;