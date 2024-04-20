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
}
