namespace IConnet.Presale.WebApp.Components.Pages;

public class TrackingPageBase : StatusTrackingPageBase, IPageNavigation
{
    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("tracking-opportunity", PageNavName.Tracking, PageRoute.Tracking);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }
}
