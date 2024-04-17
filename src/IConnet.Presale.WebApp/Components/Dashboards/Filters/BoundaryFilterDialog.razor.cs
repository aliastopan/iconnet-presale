namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class BoundaryFilterDialog : IDialogContentComponent<BoundaryFilterMode>
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public BoundaryFilterMode Content { get; set; } = default!;
    protected BoundaryFilterMode Boundary => Content;

    protected async Task SaveAsync()
    {
        LogSwitch.Debug("Save Filters");

        await Dialog.CloseAsync();
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
