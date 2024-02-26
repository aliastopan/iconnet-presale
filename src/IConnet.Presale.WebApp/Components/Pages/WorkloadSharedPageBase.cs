namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadSharedPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    private readonly string _pageName = "Workload (shared) page";

    protected string GridTemplateCols => GetGridTemplateCols();

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(WorkloadSharedPage());

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyImportVerified;

        await base.OnInitializedAsync();

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected override async Task OnUpdateWorkloadAsync(string message)
    {
        await base.OnUpdateWorkloadAsync(message);

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.DurasiTidakLanjutPx}px
            {ColumnWidth.NamaPemohonPx}px
            {ColumnWidth.IdPlnPx}px
            {ColumnWidth.LayananPx}px
            {ColumnWidth.SumberPermohonanPx}px
            {ColumnWidth.StatusPermohonanPx}px
            {ColumnWidth.NamaAgenPx}px
            {ColumnWidth.EmailAgenPx}px
            {ColumnWidth.TelpAgenPx}px
            {ColumnWidth.MitraAgenPx}px
            {ColumnWidth.SplitterPx}px
            {ColumnWidth.TelpPemohonPx}px
            {ColumnWidth.EmailPemohonPx}px
            {ColumnWidth.NikPemohonPx}px
            {ColumnWidth.NpwpPemohonPx}px
            {ColumnWidth.KeteranganPx}px
            {ColumnWidth.AlamatPemohonPx}px
            {ColumnWidth.RegionalPx}px
            {ColumnWidth.KantorPerwakilanPx}px
            {ColumnWidth.ProvinsiPx}px
            {ColumnWidth.KabupatenPx}px
            {ColumnWidth.KecamatanPx}px
            {ColumnWidth.KelurahanPx}px
            {ColumnWidth.LatitudePx}px
            {ColumnWidth.LongitudePx}px;";
    }

    private static TabNavigationModel WorkloadSharedPage()
    {
        return new TabNavigationModel("workload-shared", "Workload", PageRoute.WorkloadShared);
    }
}
