namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadSharedPageBase : WorkloadPageBase, IPageNavigation
{
    private bool _firstLoad = true;
    private IQueryable<WorkPaper>? _filteredWorkPapers;

    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("workload-shared", PageNavName.WorkloadShared, PageRoute.WorkloadShared);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.WorkloadShared;
        PresaleDataFilter = PresaleDataFilter.All;

        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected override async Task OnUpdateWorkloadAsync(string message)
    {
        await base.OnUpdateWorkloadAsync(message);

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        if (FilterComponent.IsFiltered)
        {
            if (_filteredWorkPapers is null || _firstLoad)
            {
                _firstLoad = false;

                return FilterComponent.FilterWorkPapers(base.WorkPapers)?
                    .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);
            }

            return _filteredWorkPapers;
        }

        _filteredWorkPapers = FilterComponent.FilterWorkPapers(base.WorkPapers)?
            .OrderByDescending(x => x.ApprovalOpportunity.TglPermohonan);

        ColumnWidth.SetColumnWidth(_filteredWorkPapers);

        FilterComponent.IsFiltered = true;

        return _filteredWorkPapers;
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.WorkPaperLevelPx}px
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.InChargeImportPx}px
            {ColumnWidth.InChargeVerificationPx}px
            {ColumnWidth.DurasiTidakLanjutPx}px
            {ColumnWidth.KeteranganVerifikasiPx}px
            {ColumnWidth.NamaPemohonPx + 200}px
            {ColumnWidth.TelpPemohonPx + 180}px
            {ColumnWidth.EmailPemohonPx + 200}px
            {ColumnWidth.IdPlnPx + 180}px
            {ColumnWidth.AlamatPemohonPx + 200}px
            {ColumnWidth.ValidasiShareLocPx}px
            {ColumnWidth.NikPemohonPx}px
            {ColumnWidth.NpwpPemohonPx}px
            {ColumnWidth.KeteranganPx}px
            {ColumnWidth.TglChatCallMulaiPx}px
            {ColumnWidth.TglChatCallResponsPx}px
            {ColumnWidth.HelpdeskInChargePx}px
            {ColumnWidth.LinkChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.StatusApprovalPx}px
            {ColumnWidth.RootCausePx}px
            {ColumnWidth.TglApprovalPx}px
            {ColumnWidth.PlanningAssetCoverageInChargePx}px
            {ColumnWidth.KeteranganApprovalPx}px
            {ColumnWidth.LayananPx}px
            {ColumnWidth.SumberPermohonanPx}px
            {ColumnWidth.StatusPermohonanPx}px
            {ColumnWidth.NamaAgenPx}px
            {ColumnWidth.EmailAgenPx}px
            {ColumnWidth.TelpAgenPx}px
            {ColumnWidth.MitraAgenPx}px
            {ColumnWidth.SplitterPx}px
            {ColumnWidth.SplitterGantiPx}px
            {ColumnWidth.RegionalPx}px
            {ColumnWidth.KantorPerwakilanPx}px
            {ColumnWidth.ProvinsiPx}px
            {ColumnWidth.KabupatenPx}px
            {ColumnWidth.KecamatanPx}px
            {ColumnWidth.KelurahanPx}px
            {ColumnWidth.LatitudePx}px
            {ColumnWidth.LongitudePx}px;";
    }
}
