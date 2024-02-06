namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter]
    public TabNavigation Tab { get; set; }

    [Parameter]
    public TabNavigationManager Manager { get; set; } = default!;

    protected void CloseTab()
    {
        Manager.CloseTab(Tab);
    }

    protected void ChangeTab()
    {
        Manager.ChangeTab(Tab);
    }
}
