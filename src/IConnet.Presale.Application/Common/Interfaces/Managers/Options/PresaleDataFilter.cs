namespace IConnet.Presale.Application.Common.Interfaces.Managers.Options;

public enum PresaleDataFilter
{
    All,
    OnlyImportUnverified,
    OnlyImportInvalid,
    OnlyImportArchived,
    OnlyImportVerified,
    OnlyValidating,
    OnlyWaitingApproval,
    OnlyDoneProcessing,
}
