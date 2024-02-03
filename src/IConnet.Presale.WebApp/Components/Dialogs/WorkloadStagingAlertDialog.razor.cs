namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class WorkloadStagingAlertDialog : IDialogContentComponent<WorkPaper>
{
    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}