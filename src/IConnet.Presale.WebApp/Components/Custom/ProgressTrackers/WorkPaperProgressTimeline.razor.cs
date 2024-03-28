namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTimeline : ComponentBase
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    protected bool IsAtLeastImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;
    protected bool IsAtLeastReinstated => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Reinstated;
    protected bool IsAtLeastImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;
    protected bool IsAtLeastValidating => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Validating;
    protected bool IsAtLeastWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool IsAtLeastDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected string GetImportTimestamp()
    {
        string dateTimeString = $"{WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi.ToReadableFormat()}";

        return !IsAtLeastImportUnverified
            ? ""
            : dateTimeString;
    }

    protected string GetVerificationTimestamp()
    {
        bool hasSignatureVerification = !WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastValidating && hasSignatureVerification)
            ? ""
            : dateTimeString;
    }

    protected string GetValidationTimestamp()
    {
        bool hasSignatureChatCallRespons = !WorkPaper!.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastWaitingApproval && hasSignatureChatCallRespons)
            ? ""
            : dateTimeString;
    }

    protected string GetApprovalTimestamp()
    {
        bool hasSignatureApproval = !WorkPaper!.ProsesApproval.SignatureApproval.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ProsesApproval.SignatureApproval.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastDoneProcessing && hasSignatureApproval)
            ? ""
            : dateTimeString;
    }

    protected string GetImportTimelineStepCss()
    {
        return IsAtLeastImportUnverified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    protected string GetVerificationTimelineStepCss()
    {
        return IsAtLeastReinstated
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    protected string GetValidatingTimelineStepCss()
    {
        return IsAtLeastImportVerified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    protected string GetApprovalTimelineStepCss()
    {
        return IsAtLeastWaitingApproval
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    protected Icon GetImportHeaderIcon()
    {
        string color = IsAtLeastImportUnverified
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    protected Icon GetVerificationHeaderIcon()
    {
        string color = IsAtLeastReinstated
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    protected Icon GetValidatingHeaderIcon()
    {
        string color = IsAtLeastImportVerified
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    protected Icon GetApprovalHeaderIcon()
    {
        string color = IsAtLeastImportVerified
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    // protected Icon GetHeaderIcon()
    // {
    //     return new Icons.Filled.Size20.Circle().WithColor("var(--soft-black)");
    // }
}
