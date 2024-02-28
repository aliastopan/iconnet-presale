namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum WorkPaperLevel
{
    ImportUnverified = 1,
    ImportInvalid = 2,
    ImportArchived = 4,
    ImportVerified = 8,
    Validating = 16,
    WaitingApproval = 32,
    DoneProcessing = 64
}
