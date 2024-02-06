namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter]
    public TabNavigation Tab { get; set; }

    [Parameter]
    public TabNavigationManager Manager { get; set; } = default!;

    protected void CloseTab()
    {
        Log.Warning("Closing {0}", Tab.Id);
    }

    protected void ChangeTab()
    {
        Manager.ChangeTab(Tab);
    }
}
