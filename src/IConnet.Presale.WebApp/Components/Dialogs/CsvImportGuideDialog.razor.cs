namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class CsvImportGuideDialog : IDialogContentComponent
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
