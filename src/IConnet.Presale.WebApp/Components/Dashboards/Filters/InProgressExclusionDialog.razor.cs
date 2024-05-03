namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class InProgressExclusionDialog : IDialogContentComponent<InProgressExclusionModel>
{
    [Parameter]
    public InProgressExclusionModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnExclusionChanged(WorkPaperLevel workPaperLevel, bool inclusion)
    {
        if (Content.Inclusion.Count == 1)
        {
            return;
        }

        if (inclusion)
        {
            Content.Inclusion.Add(workPaperLevel);
        }
        else
        {
            Content.Inclusion.Remove(workPaperLevel);
        }
    }

    protected string GetInProgressDisplay(WorkPaperLevel workPaperLevel)
    {
        switch (workPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "Verifikasi";
            case WorkPaperLevel.Reinstated:
                return "Verifikasi (Reset)";
            case WorkPaperLevel.ImportVerified:
                return "Chat/Call Pick-Up";
            case WorkPaperLevel.Validating:
                return "Validasi";
            case WorkPaperLevel.WaitingApproval:
                return "Approval";
            default:
                throw new NotImplementedException("Invalid In-Progress Report Target");
        }
    }
}
