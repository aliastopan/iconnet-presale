using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadSharedPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    protected FilterForm FilterComponent { get; set; } = default!;
    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("workload-shared", PageNavName.WorkloadShared, PageRoute.WorkloadShared);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.WorkloadShared;
        WorkloadFilter = WorkloadFilter.All;

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

        IQueryable<WorkPaper>? workPapers = FilterComponent.FilterWorkPapers(base.WorkPapers);

        ColumnWidth.SetColumnWidth(workPapers);
        return workPapers;
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.WorkPaperLevelPx + ColumnWidth.OffsetPx}px
            {ColumnWidth.IdPermohonanPx + ColumnWidth.OffsetPx}px
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
}
