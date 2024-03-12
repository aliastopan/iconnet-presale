namespace IConnet.Presale.WebApp.Components.Custom.ProgressTrackers;

public partial class WorkPaperProgressTracker : ComponentBase
{
    [Parameter]
    public WorkPaper? WorkPaper { get; set; }

    private readonly Icon _circleIcon = new Icons.Filled.Size20.Circle();
    private readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    // protected bool LessThanImportUnverified => WorkPaper?.WorkPaperLevel < WorkPaperLevel.ImportUnverified;
    // protected bool EqualToImportUnverified => WorkPaper?.WorkPaperLevel == WorkPaperLevel.ImportUnverified;
    // protected bool MoreThanImportUnverified => WorkPaper?.WorkPaperLevel > WorkPaperLevel.ImportUnverified;

    protected bool ProceedImportUnverified => WorkPaper?.WorkPaperLevel >= WorkPaperLevel.ImportUnverified;

    protected Icon GetImportStepIcon => GetStepIcon(WorkPaperLevel.ImportUnverified);

    protected Icon GetStepIcon(WorkPaperLevel stepLevel)
    {
        if (WorkPaper?.WorkPaperLevel > stepLevel)
        {
            return _checkmarkIcon.WithColor("var(--accent-fill-rest)");
        }
        else if (WorkPaper?.WorkPaperLevel == stepLevel)
        {
            return _circleIcon.WithColor("var(--accent-fill-rest)");

        }
        else
        {
            return _circleIcon.WithColor("var(--info)");
        }
    }

}
