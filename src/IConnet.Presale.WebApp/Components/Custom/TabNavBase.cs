namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter] public TabNavigation Tab { get; set; }
    [Parameter] public EventCallback<TabNavigation> OnTabChange { get; set; }
    [Parameter] public EventCallback<TabNavigation> OnTabClose { get; set; }

    protected async Task ChangeTabAsync()
    {
        await OnTabChange.InvokeAsync(Tab);
    }

    protected async Task CloseTabAsync()
    {
        await OnTabClose.InvokeAsync(Tab);
    }
}
