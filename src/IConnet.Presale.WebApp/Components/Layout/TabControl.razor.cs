namespace IConnet.Presale.WebApp.Components.Layout;

public partial class TabControl : ComponentBase
{
    [Parameter]
    public TabNavigationManager TabNavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        TabNavigationManager.StateHasChanged(this.StateHasChanged);
    }

    public void ChangeTab(TabNavigationModel tabNavigation)
    {
        TabNavigationManager.ChangeTab(tabNavigation);
    }

    public void CloseTab(TabNavigationModel tabNavigation)
    {
        TabNavigationManager.CloseTab(tabNavigation);
        this.StateHasChanged();
    }

    public void NavigateBack()
    {
        TabNavigationManager.NavigateBack();
    }
}
