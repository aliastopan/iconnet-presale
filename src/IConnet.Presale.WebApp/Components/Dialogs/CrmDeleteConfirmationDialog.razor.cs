namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class CrmDeleteConfirmationDialog : IDialogContentComponent
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task ConfirmDeleteAsync()
    {
        await Dialog.CloseAsync();
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
