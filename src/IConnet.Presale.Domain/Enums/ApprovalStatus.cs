namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum ApprovalStatus
{
    OnProgress = 0,
    ClosedLost = 1,
    Rejected = 2,
    Expansion = 4,
    Approved = 64
}
