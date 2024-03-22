namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum ApprovalStatus
{
    InProgress = 0,
    CloseLost = 1,
    Reject = 2,
    Expansion = 4,
    Approve = 64
}
