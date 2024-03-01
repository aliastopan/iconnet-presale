namespace IConnet.Presale.WebApp.Components.Pages;

public class HomePageBase : ComponentBase, IPageNavigation
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(PageDeclaration());

        base.OnInitialized();
    }

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("home", PageNavName.Home, PageRoute.Home);
    }
}
