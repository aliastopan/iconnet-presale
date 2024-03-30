namespace IConnet.Presale.WebApp.Components.Pages;

public class TrackingPageBase : TrackerPageBase, IPageNavigation
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

    protected string? GetImportDate()
    {
        if (WorkPaper is null)
        {
            return null;
        }

        return WorkPaper.ApprovalOpportunity.SignatureImport.TglAksi.Date.ToString();
    }
}
