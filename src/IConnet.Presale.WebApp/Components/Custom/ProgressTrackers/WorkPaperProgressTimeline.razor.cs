namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTimeline : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    protected bool IsAtLeastImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;
    // protected bool IsAtLeastReinstated => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Reinstated;
    protected bool IsAtLeastImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;
    // protected bool IsAtLeastValidating => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Validating;
    protected bool IsAtLeastWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool IsAtLeastDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected bool HasChatCallMulaiSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallMulai.IsEmptySignature();
    protected bool HasChatCallResponsSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature();
    protected bool HasApprovalSignature => !WorkPaper!.ProsesApproval.SignatureApproval.IsEmptySignature();

    protected bool IsClosedLost => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.CloseLost;
    protected bool IsRejected => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Reject;
    protected bool IsExpansion => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Expansion;
    protected bool IsApproved => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Approve;

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

    protected string GetPicApproval()
    {
        string username = WorkPaper!.ProsesApproval.SignatureApproval.ExtractUsernameFromAlias(out string role);

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

    public string GetApprovalElapsedTime()
    {
        DateTime approvalDateTime = WorkPaper!.ProsesApproval.SignatureApproval.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(approvalDateTime);

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
        return GetTimelineStepCss(IsAtLeastImportUnverified);
    }

    protected string GetVerificationTimelineStepCss()
    {
        return GetTimelineStepCss(IsAtLeastImportUnverified);
    }

    protected string GetValidatingTimelineStepCss()
    {
        return GetTimelineStepCss(IsAtLeastImportVerified);
    }

    protected string GetApprovalTimelineStepCss()
    {
        return GetTimelineStepCss(IsAtLeastWaitingApproval);
    }

    protected string GetResultTimelineStepCss()
    {
        return GetTimelineStepCss(IsAtLeastDoneProcessing);
    }

    private string GetTimelineStepCss(bool atLeastStep)
    {
        if (IsAtLeastDoneProcessing)
        {
            if (IsClosedLost)
            {
                return "timeline-step timeline-step-closed-lost";
            }

            if (IsRejected)
            {
                return "timeline-step timeline-step-rejected";
            }

            if (IsApproved)
            {
                return "timeline-step timeline-step-approved";
            }
        }

        return atLeastStep
            ? "timeline-step timeline-step-active"
            : "timeline-step timeline-step-pending";
    }

    protected Icon GetImportHeaderIcon()
    {
        return GetHeaderIcon(IsAtLeastImportUnverified);
    }

    protected Icon GetVerificationHeaderIcon()
    {
        return GetHeaderIcon(IsAtLeastImportUnverified);
    }

    protected Icon GetValidatingHeaderIcon()
    {
        return GetHeaderIcon(IsAtLeastImportVerified);
    }

    protected Icon GetApprovalHeaderIcon()
    {
        return GetHeaderIcon(IsAtLeastWaitingApproval);
    }

    protected Icon GetResultHeaderIcon()
    {
        return GetHeaderIcon(IsAtLeastDoneProcessing);
    }

    private Icon GetHeaderIcon(bool atLeastStep)
    {
        if (IsAtLeastDoneProcessing)
        {
            if (IsClosedLost)
            {
                return new Icons.Filled.Size20.SubtractCircle().WithColor("var(--soft-black)");
            }

            if (IsRejected)
            {
                return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
            }

            if (IsApproved)
            {
                return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");
            }
        }

        string color = atLeastStep
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

    protected string GetRootCause()
    {
        return WorkPaper!.ProsesApproval.RootCause.ToUpper();
    }
}
