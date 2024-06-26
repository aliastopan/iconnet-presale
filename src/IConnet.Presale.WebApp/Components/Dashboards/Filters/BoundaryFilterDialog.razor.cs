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
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
