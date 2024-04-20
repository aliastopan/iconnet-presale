namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class ApprovalStatusExclusionDialog : IDialogContentComponent<ApprovalStatusExclusionModel>
{
    [Parameter]
    public ApprovalStatusExclusionModel Content { get; set; } = default!;

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

    protected void OnExclusionChanged(ApprovalStatus approvalStatus, bool inclusion)
    {
        if (Content.Inclusion.Count == 1)
        {
            return;
        }

        if (inclusion)
        {
            Content.Inclusion.Add(approvalStatus);
        }
        else
        {
            Content.Inclusion.Remove(approvalStatus);
        }
    }
}
