namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTimeline : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    protected bool IsAtLeastImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;
    // protected bool IsAtLeastReinstated => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Reinstated;
    protected bool IsAtLeastImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;
    protected bool IsAtLeastValidating => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Validating;
    protected bool IsAtLeastWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool IsAtLeastDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected bool HasChatCallMulaiSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallMulai.IsEmptySignature();
    protected bool HasChatCallResponsSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature();

    protected string GetPicImport()
    {
        string username = WorkPaper!.ApprovalOpportunity.SignatureImport.ExtractUsernameFromAlias(out string role);

        return $"{username} - {role}";
    }

    protected string GetPicVerification()
    {
        string username = WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.ExtractUsernameFromAlias(out string role);

        return $"{username} - {role}";
    }

    protected string GetPicChatCallMulai()
    {
        string username = WorkPaper!.ProsesValidasi.SignatureChatCallMulai.ExtractUsernameFromAlias(out string role);

        role = role == "PH"
            ? "Helpdesk Presale"
            : role;

        return $"{username} - {role}";
    }

    protected string GetPicChatCallRespons()
    {
        string username = WorkPaper!.ProsesValidasi.SignatureChatCallRespons.ExtractUsernameFromAlias(out string role);

        role = role == "PH"
            ? "Helpdesk Presale"
            : role;

        return $"{username} - {role}";
    }

    public string GetImportElapsedTime()
    {
        DateTime importDateTime = WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(importDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    public string GetVerificationElapsedTime()
    {
        DateTime verificationDateTime = WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(verificationDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    public string GetChatCallMulaiElapsedTime()
    {
        DateTime chatCallMulaiDateTime = WorkPaper!.ProsesValidasi.SignatureChatCallMulai.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(chatCallMulaiDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    public string GetChatCallResponsElapsedTime()
    {
        DateTime chatCallResponsDateTime = WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(chatCallResponsDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

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

        return !(IsAtLeastImportVerified && hasSignatureVerification)
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
        return IsAtLeastImportUnverified
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

    protected string GetResultTimelineStepCss()
    {
        return IsAtLeastDoneProcessing
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
        string color = IsAtLeastImportUnverified
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
        string color = IsAtLeastWaitingApproval
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    protected Icon GetResultHeaderIcon()
    {
        string color = IsAtLeastDoneProcessing
            ? "var(--accent-blue)"
            : "var(--inactive-grey)";

        return new Icons.Filled.Size20.Circle().WithColor(color);
    }

    protected string GetResultString()
    {
        if (!IsAtLeastDoneProcessing)
        {
            return "STATUS PERMOHONAN";
        }

        switch (WorkPaper!.ProsesApproval.StatusApproval)
        {
            case ApprovalStatus.CloseLost:
                return "CLOSED LOST";
            case ApprovalStatus.Reject:
                return "REJECTED";
            case ApprovalStatus.Approve:
                return "APPROVED";
            default:
                return "STATUS PERMOHONAN";
        }
    }
}
