namespace IConnet.Presale.WebApp.Components.Pages;

public class HomePageBase : ComponentBase
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(HomePage());

        base.OnInitialized();
    }

    private static TabNavigation HomePage()
    {
        return new TabNavigation("home", "Home", PageRoute.Home);
    }
}
