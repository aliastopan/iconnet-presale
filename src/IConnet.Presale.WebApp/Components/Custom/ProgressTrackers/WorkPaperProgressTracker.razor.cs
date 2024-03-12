namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTracker : ComponentBase
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    private readonly string _greyedOut = "#e6e6e6";

    protected Icon ImportStepIcon => GetStepIcon(WorkPaperLevel.ImportUnverified);
    protected string ImportStepHeaderColor => GetHeaderColor(WorkPaperLevel.ImportUnverified);
    protected string ImportStepTrailColor => GetTrailColor(WorkPaperLevel.ImportUnverified);
    protected DateTime ImportDateTime => WorkPaper!.ApprovalOpportunity.SignatureImport.TglAksi;
    protected bool ProceedImportUnverified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;

    protected Icon ImportVerifiedStepIcon => GetStepIcon(WorkPaperLevel.ImportVerified);
    protected string ImportVerificationStepHeaderColor => GetHeaderColor(WorkPaperLevel.ImportVerified);
    protected string ImportVerificationStepTrailColor => GetTrailColor(WorkPaperLevel.ImportVerified);
    protected DateTime ImportVerifiedDateTime => WorkPaper!.ApprovalOpportunity.SignatureVerifikasiImport.TglAksi;
    protected bool ProceedImportVerified => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.ImportVerified;

    protected Icon ValidatingStepIcon => GetStepIcon(WorkPaperLevel.Validating, checkOnGoing: true);
    protected string ValidatingStepHeaderColor => GetHeaderColor(WorkPaperLevel.Validating);
    protected string ValidatingStepTrailColor => GetTrailColor(WorkPaperLevel.Validating, checkOnGoing: true);
    protected DateTime ValidatingDateTime => WorkPaper!.ProsesValidasi.SignatureChatCallRespons.TglAksi;
    protected bool IsValidating => WorkPaper!.WorkPaperLevel == WorkPaperLevel.Validating;
    protected bool ProceedValidating => WorkPaper!.WorkPaperLevel > WorkPaperLevel.Validating;

    protected Icon WaitingApprovalStepIcon => GetStepIcon(WorkPaperLevel.WaitingApproval, checkOnGoing: true);
    protected string WaitingApprovalStepHeaderColor => GetHeaderColor(WorkPaperLevel.WaitingApproval);
    protected string WaitingApprovalStepTrailColor => GetTrailColor(WorkPaperLevel.WaitingApproval, checkOnGoing: true);
    protected DateTime ApprovalDateTime => WorkPaper!.ProsesApproval.SignatureApproval.TglAksi;
    protected bool IsWaitingApproval => WorkPaper!.WorkPaperLevel == WorkPaperLevel.WaitingApproval;
    protected bool ProceedWaitingApproval => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.WaitingApproval;
    protected bool ProceedDoneProcessing => WorkPaper!.WorkPaperLevel >= WorkPaperLevel.DoneProcessing;

    protected Icon GetStepIcon(WorkPaperLevel stepLevel, bool checkOnGoing = false)
    {
        if (!checkOnGoing)
        {
            if (WorkPaper?.WorkPaperLevel >= stepLevel)
            {
                return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");
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
                return new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success)");
            }
            else
            {
                if (WorkPaper?.WorkPaperLevel == stepLevel)
                {
                    return new Icons.Filled.Size20.Circle().WithColor("var(--accent-fill-rest)");

                }

                return new Icons.Filled.Size20.Circle().WithColor(_greyedOut);
            }
        }
    }

    protected string GetTrailColor(WorkPaperLevel stepLevel, bool checkOnGoing = false)
    {
        if (!checkOnGoing)
        {
            if (WorkPaper?.WorkPaperLevel >= stepLevel)
            {
                return "border-left: 2px solid var(--success) !important;";
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
                return "border-left: 2px solid var(--success) !important;";
            }
            else
            {
                if (WorkPaper?.WorkPaperLevel == stepLevel)
                {
                    return "border-left: 2px solid var(--accent-fill-rest) !important;";
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
}
