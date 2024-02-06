namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter]
    public TabNavigation Tab { get; set; }

    [Parameter]
    public TabNavigationManager Manager { get; set; } = default!;

    [Parameter]
    public EventCallback OnTabClose { get; set; }

    protected async Task CloseTabAsync()
    {
        await Manager.CloseTabAsync(Tab, OnTabClose);
    }

    protected void ChangeTab()
    {
        Manager.ChangeTab(Tab);
    }
}
