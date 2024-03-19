namespace IConnet.Presale.WebApp.Components.Pages;

public class IndexPageBase : ComponentBase
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;

    private IQueryable<WorkPaper>? _workPapers;
    private readonly WorkloadColumnWidth _columnWidth = new WorkloadColumnWidth();

    public bool IsLoading { get; set; } = false;
    public bool HasSearched { get; set; } = false;
    public string IdPermohonan { get; set; } = default!;
    public WorkPaper? WorkPaper { get; set; }

    protected string GridTemplateCols => GetGridTemplateCols();
    protected IQueryable<WorkPaper>? WorkPapers => _workPapers;
    protected WorkloadColumnWidth ColumnWidth => _columnWidth;

    protected async Task OnIdPermohonanSearchChanged(string idPermohonan)
    {
        IsLoading = true;
        HasSearched = true;

        IdPermohonan = idPermohonan.SanitizeOnlyAlphanumeric();
        WorkPaper = await WorkloadManager.SearchWorkPaperAsync(IdPermohonan);

        await Task.CompletedTask;

        if (WorkPaper is not null)
        {
            List<WorkPaper> result = [WorkPaper];

            _workPapers = result.AsQueryable();
            ColumnWidth.SetColumnWidth(_workPapers);
        }
        else
        {
            List<WorkPaper> result = [];

            _workPapers = result.AsQueryable();
            HasSearched = false;
        }

        IsLoading = false;
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.WorkPaperLevelPx}px
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.DurasiTidakLanjutPx}px
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
