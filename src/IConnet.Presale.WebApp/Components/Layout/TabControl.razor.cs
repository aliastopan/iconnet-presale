namespace IConnet.Presale.WebApp.Components.Layout;

public partial class TabControl
{
    [Parameter]
    public TabNavigationManager TabNavigationManager { get; set; } = default!;

    public void ChangeTab(TabNavigation tab)
    {
        TabNavigationManager.ChangeTab(tab);
    }

    public void SelectTab(TabNavigation tab)
    {
        Log.Warning("Tab selected: {0}", tab.Label);
        // RefreshTabControl()
    }

    public void RefreshTabControl()
    {
        StateHasChanged();
        Log.Warning("Refreshing Tab Control");
    }
}
