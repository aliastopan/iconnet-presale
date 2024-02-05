using IConnet.Presale.WebApp.Components.Pages;

namespace IConnet.Presale.WebApp.Components.Layout;

public class NavMenuBase : ComponentBase
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

    public bool IsExpanded { get; set; } = true;

    public void OnClick()
    {
        Log.Warning("OnClick");
    }

    public void OnHomePageNavigate()
    {
        TabNavigationManager.SelectTab(HomePage());
    }

    public void OnCrmImportPageNavigate()
    {
        TabNavigationManager.SelectTab(CrmImportPage());
    }

    public void OnCrmVerificationPageNavigate()
    {
        TabNavigationManager.SelectTab(CrmVerificationPage());
    }

    public static TabNavigation HomePage()
    {
        return new TabNavigation("home", "Home", PageRoute.Home);
    }

    public static TabNavigation CrmImportPage()
    {
        return new TabNavigation("crm-import", "Import CRM", PageRoute.CrmImport);
    }

    public static TabNavigation CrmVerificationPage()
    {
        return new TabNavigation("crm-verification", "Verifikasi CRM", PageRoute.CrmImportVerification);
    }
}
