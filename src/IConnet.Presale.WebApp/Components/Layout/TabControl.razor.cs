namespace IConnet.Presale.WebApp.Components.Layout;

public partial class TabControl : ComponentBase
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

    public void NavigateBack()
    {
        TabNavigationManager.NavigateBack();
    }

    private void RefreshTabControl()
    {
        StateHasChanged();
        // Log.Warning("Refreshing Tab Control");
    }
}
