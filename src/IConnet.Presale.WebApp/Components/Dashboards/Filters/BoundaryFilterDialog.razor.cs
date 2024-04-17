namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class BoundaryFilterDialog : IDialogContentComponent
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync();
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
