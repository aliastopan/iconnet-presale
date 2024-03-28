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
    public bool IsAtLeastDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    public string GetImportTimelineStepCss()
    {
        return IsAtLeastImportUnverified
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    public string GetImportTimestamp()
    {
        string dateTimeString = $"{WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi.ToReadableFormat()}";

        return !IsAtLeastImportUnverified
            ? ""
            : dateTimeString;
    }

    public string GetVerificationTimestamp()
    {
        bool hasSignatureVerification = !WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastValidating && hasSignatureVerification)
            ? ""
            : dateTimeString;
    }

    public string GetValidationTimestamp()
    {
        bool hasSignatureChatCallRespons = !WorkPaper!.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastWaitingApproval && hasSignatureChatCallRespons)
            ? ""
            : dateTimeString;
    }

    public string GetApprovalTimestamp()
    {
        bool hasSignatureApproval = !WorkPaper!.ProsesApproval.SignatureApproval.IsEmptySignature();
        string dateTimeString = $"{WorkPaper!.ProsesApproval.SignatureApproval.TglAksi.ToReadableFormat()}";

        return !(IsAtLeastDoneProcessing && hasSignatureApproval)
            ? ""
            : dateTimeString;
    }

    public string GetVerificationTimelineStepCss()
    {
        return IsAtLeastReinstated
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
