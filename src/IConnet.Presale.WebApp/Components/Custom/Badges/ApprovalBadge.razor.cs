namespace IConnet.Presale.WebApp.Components.Custom.Badges;

public partial class ApprovalBadge : ComponentBase
{
    [Parameter]
    public ApprovalStatus ApprovalStatus { get; set; } = default!;

    protected string GetCssBadge()
    {
        switch (ApprovalStatus)
        {
            case ApprovalStatus.OnProgress:
                return "approval-badge-on-progress";
            case ApprovalStatus.ClosedLost:
                return "approval-badge-closed-lost";
            case ApprovalStatus.Reject:
                return "approval-badge-reject";
            case ApprovalStatus.Expansion:
                return "approval-badge-expansion";
            case ApprovalStatus.Approve:
                return "approval-badge-approved";
            default:
                return "approval-badge-neutral";
        }
    }

    protected string GetApprovalStatusString()
    {
        return EnumProcessor.EnumToDisplayString(ApprovalStatus);
    }
}
