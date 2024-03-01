namespace IConnet.Presale.Application.Common.Interfaces.Managers.Options;

public enum WorkloadFilter
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
