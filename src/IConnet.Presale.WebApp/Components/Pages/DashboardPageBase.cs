using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : ComponentBase, IPageNavigation
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard-wip", PageNavName.Dashboard, PageRoute.Dashboard);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);
    }

}
