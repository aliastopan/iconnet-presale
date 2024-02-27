namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum WorkPaperLevel
{
    ImportUnverified,
    ImportVerified,
    ImportInvalid,
    ImportArchived,
    Validating,
    WaitingApproval,
    DoneProcessing
}
