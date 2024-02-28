namespace IConnet.Presale.Application.Common.Interfaces.Managers.Options;

public enum CacheFetchMode
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
