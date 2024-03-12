namespace IConnet.Presale.WebApp.Components.Pages;

public class HomePageBase : ComponentBase, IPageNavigation
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;

    public string IdPermohonan { get; set; } = default!;
    public WorkPaper? WorkPaper { get; set; }

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("home", PageNavName.Home, PageRoute.Home);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected async Task OnIdPermohonanSearchChanged(string idPermohonan)
    {
        IdPermohonan = idPermohonan;
        WorkPaper = await WorkloadManager.SearchWorkPaper(IdPermohonan);

        await Task.CompletedTask;
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
