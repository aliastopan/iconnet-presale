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

    public void OnWorkloadSharedPageNavigate()
    {
        TabNavigationManager.SelectTab(WorkloadSharedPage());
    }

    public void OnHelpdeskStagingPageNavigate()
    {
        TabNavigationManager.SelectTab(HelpdeskStagingPage());
    }

    public void OnHelpdeskPageNavigate()
    {
        TabNavigationManager.SelectTab(HelpdeskPage());
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

    public static TabNavigation WorkloadSharedPage()
    {
        return new TabNavigation("workload-shared", "Workload", PageRoute.WorkloadShared);
    }

    public static TabNavigation HelpdeskStagingPage()
    {
        return new TabNavigation("helpdesk-staging", "Staging", PageRoute.HelpdeskStaging);
    }

    public static TabNavigation HelpdeskPage()
    {
        return new TabNavigation("helpdesk", "Helpdesk", PageRoute.Helpdesk);
    }
}
