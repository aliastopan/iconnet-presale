namespace IConnet.Presale.Shared.Contracts.Common;

public record AddRootCauseRequest(int Order, string Cause, string Classification) : IAddRootCauseModel;