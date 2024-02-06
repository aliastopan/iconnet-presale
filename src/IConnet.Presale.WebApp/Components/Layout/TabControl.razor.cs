namespace IConnet.Presale.WebApp.Components.Layout;

public partial class TabControl
{
    [Parameter]
    public TabNavigationManager TabNavigationManager { get; set; } = default!;

    public void ChangeTab(TabNavigation tab)
    {
        TabNavigationManager.ChangeTab(tab);
    }

    public void CloseTab(TabNavigation tab)
    {
        TabNavigationManager.CloseTab(tab);
        RefreshTabControl();
    }

    private void RefreshTabControl()
    {
        StateHasChanged();
        Log.Warning("Refreshing Tab Control");
    }
}
