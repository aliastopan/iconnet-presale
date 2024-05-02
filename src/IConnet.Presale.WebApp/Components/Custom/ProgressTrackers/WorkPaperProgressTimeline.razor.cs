namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTimeline : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    protected bool IsInvalid => WorkPaper!.WorkPaperLevel == WorkPaperLevel.ImportInvalid;
    protected bool IsAtLeastImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;
    // protected bool IsAtLeastReinstated => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Reinstated;
    protected bool IsAtLeastImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;
    // protected bool IsAtLeastValidating => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.Validating;
    protected bool IsAtLeastWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool IsAtLeastDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected bool HasChatCallMulaiSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallMulai.IsEmptySignature();
    protected bool HasChatCallResponsSignature => !WorkPaper!.ProsesValidasi.SignatureChatCallRespons.IsEmptySignature();
    protected bool HasApprovalSignature => !WorkPaper!.ProsesApproval.SignatureApproval.IsEmptySignature();

    protected bool IsDirectlyApproved => WorkPaper!.ProsesApproval.IsDirectlyApproved();

    protected bool IsClosedLost => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.CloseLost;
    protected bool IsRejected => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Reject;
    protected bool IsExpansion => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Expansion;
    protected bool IsApproved => WorkPaper!.ProsesApproval.StatusApproval == ApprovalStatus.Approve;

    protected string GetSalesAgent()
    {
        return WorkPaper!.ApprovalOpportunity.Agen.NamaLengkap;
    }

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

    protected string GetImportElapsedTime()
    {
        DateTime importDateTime = WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(importDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    protected string GetVerificationElapsedTime()
    {
        DateTime verificationDateTime = WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(verificationDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    protected string GetChatCallMulaiElapsedTime()
    {
        DateTime chatCallMulaiDateTime = WorkPaper!.ProsesValidasi.SignatureChatCallMulai.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(chatCallMulaiDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    protected string GetChatCallResponsElapsedTime()
    {
        DateTime chatCallResponsDateTime = WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(chatCallResponsDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    protected string GetApprovalElapsedTime()
    {
        DateTime approvalDateTime = WorkPaper!.ProsesApproval.SignatureApproval.TglAksi;
        TimeSpan elapsedTime = DateTimeService.GetElapsedTime(approvalDateTime);

        return elapsedTime.ToReadableDateTime(useLowerCaseNotation: true);
    }

    protected string GetDirectApproval()
    {
        return WorkPaper!.ProsesApproval.DirectApproval;
    }

    protected string GetInitialTimestamp()
    {
        return $"{WorkPaper!.ApprovalOpportunity.TglPermohonan.ToReadableFormat()}";
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

    protected string GetInitialTimelineStepCss()
    {
        if (IsInvalid)
        {
            return "timeline-step timeline-step-rejected";
        }

        return GetTimelineStepCss(IsAtLeastImportUnverified);
    }

    protected string GetImportTimelineStepCss()
    {
        if (IsInvalid)
        {
            return "timeline-step timeline-step-rejected";
        }

        return GetTimelineStepCss(IsAtLeastImportUnverified);
    }

    protected string GetVerificationTimelineStepCss()
    {
        if (IsInvalid)
        {
            return "timeline-step timeline-step-rejected";
        }

        return GetTimelineStepCss(IsAtLeastImportUnverified);
    }

    protected string GetValidatingTimelineStepCss()
    {
        if (IsDirectlyApproved)
        {
            return "timeline-step timeline-step-pending";
        }

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

    protected Icon GetInitialHeaderIcon()
    {
        if (IsInvalid)
        {
            return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
        }

        return GetHeaderIcon(IsAtLeastImportUnverified);
    }

    protected Icon GetImportHeaderIcon()
    {
        if (IsInvalid)
        {
            return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
        }

        return GetHeaderIcon(IsAtLeastImportUnverified);
    }

    protected Icon GetVerificationHeaderIcon()
    {
        if (IsInvalid)
        {
            return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
        }

        return GetHeaderIcon(IsAtLeastImportUnverified);
    }

    protected Icon GetValidatingHeaderIcon()
    {
        if (IsDirectlyApproved)
        {
            return new Icons.Filled.Size20.Circle().WithColor("var(--inactive-grey)");
        }

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
                {
                    if (IsDirectlyApproved)
                    {
                        return "APPROVED (DIRECT)";
                    }
                    else
                    {
                        return "APPROVED";
                    }
                }
            default:
                return "STATUS PERMOHONAN";
        }
    }

    protected string GetRootCause()
    {
        return WorkPaper!.ProsesApproval.RootCause.ToUpper();
    }

    protected string GetKeteranganApproval()
    {
        return WorkPaper!.ProsesApproval.Keterangan;
    }
}
