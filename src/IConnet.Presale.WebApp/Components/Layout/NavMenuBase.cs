using Microsoft.AspNetCore.Components.Web;
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

    public void OnHomePageNavigate(MouseEventArgs _)
    {
        TabNavigationManager.SelectTab(HomePage());
    }

    public void OnCrmImportPageNavigate(MouseEventArgs _)
    {
        TabNavigationManager.SelectTab(CrmImportPage());
    }

    public void OnCrmVerificationPageNavigate(MouseEventArgs _)
    {
        TabNavigationManager.SelectTab(CrmVerificationPage());
    }

    public void OnWorkloadSharedPageNavigate(MouseEventArgs _)
    {
        TabNavigationManager.SelectTab(WorkloadSharedPage());
    }

    public void OnHelpdeskStagingPageNavigate(MouseEventArgs _)
    {
        TabNavigationManager.SelectTab(HelpdeskStagingPage());
    }

    public void OnHelpdeskPageNavigate(MouseEventArgs _)
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
