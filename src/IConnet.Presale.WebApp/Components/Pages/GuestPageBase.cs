using IConnet.Presale.WebApp.Components.Custom;

namespace IConnet.Presale.WebApp.Components.Pages;

public class GuestPageBase : StatusTrackingPageBase
{
    public CustomErrorBoundary? ErrorBoundary { get; set; }

    protected override void OnInitialized()
    {
        if (ErrorBoundary?.CurrentException is not null)
        {
            Log.Information("Recovering from {exception}", ErrorBoundary.CurrentException.GetType().Name);
            ErrorBoundary?.Recover();
        }
    }
}
