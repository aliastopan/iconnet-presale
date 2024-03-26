namespace IConnet.Presale.Application.Common.Interfaces.Managers.Options;

public enum PresaleDataFilter
{
    All,
    OnlyImportUnverified,
    OnlyImportInvalid,
    OnlyReinstated,
    OnlyImportVerified,
    OnlyValidating,
    OnlyWaitingApproval,
    OnlyDoneProcessing,
}
