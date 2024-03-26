namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ResetPresaleDataDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected bool IsRejected => Content.ProsesApproval.StatusApproval != ApprovalStatus.Approve;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected async Task ResetDataPresaleAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = Content.ApprovalOpportunity.IdPermohonan,
            TrapFocus = true,
            PreventDismissOnOverlayClick = true,
            Width = "650px",
        };

        var dialog = await DialogService.ShowDialogAsync<ResetPresaleDataConfirmationDialog>(parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await Dialog.CloseAsync(Content);
        }
    }
}
