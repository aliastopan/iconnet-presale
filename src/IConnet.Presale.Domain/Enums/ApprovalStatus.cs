namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum ApprovalStatus
{
    OnProgress = 0,
    ClosedLost = 1,
    Reject = 2,
    Expansion = 4,
    Approve = 64
}
