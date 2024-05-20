namespace IConnet.Presale.Shared.Contracts.Common;

public record EditRootCauseRequest(Guid RootCauseId, string RootCause, string Classification)
    : IEditRootCauseModel;
