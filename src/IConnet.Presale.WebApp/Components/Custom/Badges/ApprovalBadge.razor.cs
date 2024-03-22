namespace IConnet.Presale.WebApp.Components.Custom.Badges;

public partial class ApprovalBadge : ComponentBase
{
    [Parameter]
    public ApprovalStatus ApprovalStatus { get; set; } = default!;

    protected string GetCssBadge()
    {
        switch (ApprovalStatus)
        {
            case ApprovalStatus.InProgress:
                return "approval-badge-in-progress";
            case ApprovalStatus.CloseLost:
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
        switch (ApprovalStatus)
        {
            case ApprovalStatus.InProgress:
                return "In Progress";
            case ApprovalStatus.CloseLost:
                return "Closed Lost";
            case ApprovalStatus.Reject:
                return "Rejected";
            case ApprovalStatus.Expansion:
                return "Expansion";
            case ApprovalStatus.Approve:
                return "Approved";
            default:
                throw new NotImplementedException();;
        }
    }
}
