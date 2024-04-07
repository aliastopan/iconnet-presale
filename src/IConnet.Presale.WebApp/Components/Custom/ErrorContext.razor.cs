namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ErrorContext : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] public Exception Exception { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await JsRuntime.InvokeVoidAsync("console.error", Exception.StackTrace);
    }

    protected void ReloadCurrentPage()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
}
