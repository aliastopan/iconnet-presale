namespace IConnet.Presale.WebApp.Components.Layout;

public partial class TabControl : ComponentBase
{
    [Parameter]
    public TabNavigationManager TabNavigationManager { get; set; } = default!;

    protected override void OnParametersSet()
    {
        TabNavigationManager.StateHasChanged(this.StateHasChanged);
    }

    public void ChangeTab(TabNavigation tab)
    {
        TabNavigationManager.ChangeTab(tab);
    }

    public void CloseTab(TabNavigation tab)
    {
        TabNavigationManager.CloseTab(tab);
        this.StateHasChanged();
    }

    public void NavigateBack()
    {
        TabNavigationManager.NavigateBack();
    }
}
