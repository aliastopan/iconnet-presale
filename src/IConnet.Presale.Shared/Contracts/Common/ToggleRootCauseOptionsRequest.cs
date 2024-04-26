namespace IConnet.Presale.Shared.Contracts.Common;

public record ToggleRootCauseOptionsRequest(Guid RootCauseId, bool IsDeleted, bool IsOnVerification)
    : IToggleRootCauseOptions;