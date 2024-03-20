namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTracker : ComponentBase
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    private readonly string _greyedOut = "#e6e6e6";

    protected Icon ImportStepIcon => GetStepIcon(WorkPaperLevel.ImportUnverified);
    protected string ImportStepHeaderColorStyle => GetHeaderColor(WorkPaperLevel.ImportUnverified);
    protected string ImportStepTrailColorStyle => GetTrailColor(WorkPaperLevel.ImportUnverified);
    protected DateTime ImportDateTime => WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi;
    protected bool ProceedImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;

    protected Icon ImportVerifiedStepIcon => GetStepIcon(WorkPaperLevel.ImportVerified);
    protected string ImportVerificationStepHeaderColorStyle => GetHeaderColor(WorkPaperLevel.ImportVerified);
    protected string ImportVerificationStepTrailColorStyle => GetTrailColor(WorkPaperLevel.ImportVerified);
    protected DateTime ImportVerifiedDateTime => WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
    protected bool ProceedImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;

    protected Icon ValidatingStepIcon => GetStepIcon(WorkPaperLevel.Validating, checkOnGoing: true);
    protected string ValidatingStepHeaderColorStyle => GetHeaderColor(WorkPaperLevel.Validating);
    protected string ValidatingStepTrailColorStyle => GetTrailColor(WorkPaperLevel.Validating, checkOnGoing: true);
    protected DateTime ValidatingDateTime => WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi;
    protected bool IsValidating => WorkPaper!.WorkPaperLevel == WorkPaperLevel.Validating;
    protected bool ProceedValidating => WorkPaper!.WorkPaperLevel > WorkPaperLevel.Validating;

    protected Icon WaitingApprovalStepIcon => GetStepIcon(WorkPaperLevel.WaitingApproval, checkOnGoing: true);
    protected string WaitingApprovalStepHeaderColorStyle => GetHeaderColor(WorkPaperLevel.WaitingApproval);
    protected string WaitingApprovalStepTrailColorStyle => GetTrailColor(WorkPaperLevel.WaitingApproval, checkOnGoing: true);
    protected DateTime ApprovalDateTime => WorkPaper!.ProsesApproval.SignatureApproval.TglAksi;
    protected bool IsWaitingApproval => WorkPaper!.WorkPaperLevel == WorkPaperLevel.WaitingApproval;
    protected bool ProceedWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool ProceedDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected Icon GetStepIcon(WorkPaperLevel stepLevel, bool checkOnGoing = false)
    {
        if (WorkPaper?.WorkPaperLevel == WorkPaperLevel.DoneProcessing
            && !IsApproved())
        {
            return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
        }

        if (!checkOnGoing)
        {
            if (WorkPaper?.WorkPaperLevel >= stepLevel)
            {
                return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");
            }
            else
            {
                return new Icons.Filled.Size20.Circle().WithColor(_greyedOut);
            }
        }
        else
        {
            if (WorkPaper?.WorkPaperLevel > stepLevel)
            {
                return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");
            }
            else
            {
                if (WorkPaper?.WorkPaperLevel == stepLevel)
                {
                    return new Icons.Filled.Size20.QuestionCircle().WithColor("var(--info-grey)");

                }

                return new Icons.Filled.Size20.Circle().WithColor(_greyedOut);
            }
        }
    }

    protected string GetTrailColor(WorkPaperLevel stepLevel, bool checkOnGoing = false)
    {
        if (WorkPaper?.WorkPaperLevel == WorkPaperLevel.DoneProcessing
            && !IsApproved())
        {
            return "border-left: 2px solid var(--error-red) !important;";
        }

        if (!checkOnGoing)
        {
            if (WorkPaper?.WorkPaperLevel >= stepLevel)
            {
                return "border-left: 2px solid var(--success-green) !important;";
            }
            else
            {
                return $"border-left: 2px solid {_greyedOut} !important;";
            }
        }
        else
        {
            if (WorkPaper?.WorkPaperLevel > stepLevel)
            {
                return "border-left: 2px solid var(--success-green) !important;";
            }
            else
            {
                if (WorkPaper?.WorkPaperLevel == stepLevel)
                {
                    return "border-left: 2px solid var(--info-grey) !important;";
                }

                return $"border-left: 2px solid {_greyedOut} !important;";
            }
        }
    }

    protected string GetHeaderColor(WorkPaperLevel stepLevel)
    {
        if (WorkPaper?.WorkPaperLevel >= stepLevel)
        {
            return "color: var(--soft-black)";
        }
        else
        {
            return $"color: {_greyedOut};";
        }
    }

    protected Icon GetResultStepIcon()
    {
        if (IsApproved())
        {
            return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");
        }
        else
        {
            return new Icons.Filled.Size20.DismissCircle().WithColor("var(--error-red)");
        }
    }

    protected string GetResultTrailColor()
    {
        if (IsApproved())
        {
            return "border-left: 2px solid var(--success-green) !important;";
        }
        else
        {
            return "border-left: 2px solid var(--error-red) !important;";
        }
    }

    protected string GetApprovalStatus()
    {
        return WorkPaper!.ProsesApproval.StatusApproval.ToString();
    }

    protected string GetApprovalStatusLabelStyle()
    {
        return IsApproved()
            ? "background-color: var(--success-green); color: white;"
            : "background-color: var(--error-red); color: white;";
    }

    private bool IsApproved()
    {
        return WorkPaper?.ProsesApproval.StatusApproval == ApprovalStatus.Approve
            || WorkPaper?.ProsesApproval.StatusApproval == ApprovalStatus.Expansion;
    }
}
