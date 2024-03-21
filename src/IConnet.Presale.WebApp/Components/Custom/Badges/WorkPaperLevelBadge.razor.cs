namespace IConnet.Presale.WebApp.Components.Custom.Badges;

public partial class WorkPaperLevelBadge : ComponentBase
{
    [Parameter] public WorkPaperLevel WorkPaperLevel { get; set; } = default!;
    [Parameter] public ApprovalStatus ApprovalStatus { get; set; } = default!;

    protected string GetCssBadge()
    {
        switch (WorkPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "workpaper-import-unverified";
            case WorkPaperLevel.ImportInvalid:
                return "workpaper-import-invalid";
            case WorkPaperLevel.ImportArchived:
                return "workpaper-import-archived";
            case WorkPaperLevel.ImportVerified:
                return "workpaper-import-verified";
            case WorkPaperLevel.Validating:
                return "workpaper-validating";
            case WorkPaperLevel.WaitingApproval:
                return "workpaper-waiting-approval";
            case WorkPaperLevel.DoneProcessing:
                {
                    switch (ApprovalStatus)
                    {
                        case ApprovalStatus.CloseLost:
                            return "workpaper-done-processing-closed-lost";
                        case ApprovalStatus.Reject:
                            return "workpaper-done-processing-reject";
                        default:
                            return "workpaper-done-processing-success";
                    }
                }
            default:
                return "workpaper-neutral";
        }
    }

    protected string GetWorkPaperLevelString()
    {
        switch (WorkPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "Menunggu Verifikasi";
            case WorkPaperLevel.ImportInvalid:
                return "Invalid";
            case WorkPaperLevel.ImportArchived:
                return "Archived";
            case WorkPaperLevel.ImportVerified:
                return "Menunggu Validasi";
            case WorkPaperLevel.Validating:
                return "Proses Validasi";
            case WorkPaperLevel.WaitingApproval:
                return "Menunggu Approval";
            case WorkPaperLevel.DoneProcessing:
                return "Selesai";
            default:
                return string.Empty;
        }
    }
}
