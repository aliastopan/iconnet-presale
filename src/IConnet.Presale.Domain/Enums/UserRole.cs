namespace IConnet.Presale.Domain.Enums;

[Flags]
public enum UserRole
{
    Guest = 0,
    Helpdesk = 1,
    PlanningAssetCoverage = 2,
    Administrator = 64
}
