namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ErrorContext : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public Exception Exception { get; set; } = default!;

    protected void ReloadCurrentPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
}
