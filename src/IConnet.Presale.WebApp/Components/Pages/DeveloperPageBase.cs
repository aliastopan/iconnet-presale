namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : IndexPageBase
{
    [Inject] public IWorkPaperHttpClient WorkPaperHttpClient { get; set; } = default!;

    protected async Task ForwardingAsync()
    {
        await Task.CompletedTask;

        LogSwitch.Debug("Forwarding...");
    }
}
