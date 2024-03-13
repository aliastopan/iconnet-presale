namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum ApprovalStatus
{
    OnProgress,
    ClosedLost,
    Reject,
    Approve,
    Expansion
}
