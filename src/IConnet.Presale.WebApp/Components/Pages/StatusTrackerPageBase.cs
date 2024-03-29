namespace IConnet.Presale.WebApp.Components.Pages;

public class StatusTrackerPageBase : IndexPageBase, IPageNavigation
{
    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("status-tracker", PageNavName.StatusTracker, PageRoute.StatusTracker);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected string? GetImportDate()
    {
        if (WorkPaper is null)
        {
            return null;
        }

        return WorkPaper.ApprovalOpportunity.SignatureImport.TglAksi.Date.ToString();
    }
}
