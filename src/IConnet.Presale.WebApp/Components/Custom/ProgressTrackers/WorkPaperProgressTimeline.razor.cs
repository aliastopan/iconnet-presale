namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTimeline : ComponentBase
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    public bool IsAtLeastImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;
    public bool IsAtLeastReinstated => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Reinstated;
    public bool IsAtLeastImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;
    public bool IsAtLeastValidating => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Validating;
    public bool IsAtLeastWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;

    public string GetImportTimelineStepCss()
    {
        return IsAtLeastImportUnverified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    public string GetVerificationTimelineStepCss()
    {
        return IsAtLeastImportVerified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    public string GetValidatingTimelineStepCss()
    {
        return IsAtLeastImportVerified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    public string GetWaitingApprovalTimelineStepCss()
    {
        return IsAtLeastWaitingApproval
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }
}
