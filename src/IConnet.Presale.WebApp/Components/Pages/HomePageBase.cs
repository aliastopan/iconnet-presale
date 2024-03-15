namespace IConnet.Presale.WebApp.Components.Pages;

public class HomePageBase : IndexPageBase, IPageNavigation
{
    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("home", PageNavName.Home, PageRoute.Home);
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
