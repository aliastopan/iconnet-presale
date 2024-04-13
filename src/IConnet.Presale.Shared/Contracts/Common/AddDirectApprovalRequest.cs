namespace IConnet.Presale.Shared.Contracts.Common;

public record AddDirectApprovalRequest(int Order, string Description) : IAddDirectApprovalModel;
