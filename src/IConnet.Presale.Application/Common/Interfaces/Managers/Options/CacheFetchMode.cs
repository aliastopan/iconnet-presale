namespace IConnet.Presale.Application.Common.Interfaces.Managers.Options;

public enum CacheFetchMode
{
    All,
    OnlyImportUnverified,
    OnlyImportVerified,
    OnlyImportInvalid,
    OnlyImportArchived,
    OnlyValidating,
    OnlyWaitingApproval,
    OnlyDoneProcessing,
}
