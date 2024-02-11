using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmVerificationPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private readonly string _pageName = "CRM Verification page";

    protected FilterForm FilterComponent { get; set; } = default!;
    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => GetWorkPapers();

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(CrmVerificationPage());

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyImportUnverified;

        await base.OnInitializedAsync();

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        await OpenDialogAsync(row.Item);
    }

    protected IQueryable<WorkPaper>? GetWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        return FilterComponent.FilterWorkPapers(base.WorkPapers);
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        // Log.Warning("Import status before: {0}", workPaper.ApprovalOpportunity.StatusImport);
        var parameters = new DialogParameters()
        {
            Title = "Verifikasi Import CRM",
            TrapFocus = true,
            Width = "500px",
        };

        var isImportVerified = workPaper.ApprovalOpportunity.StatusImport == ImportStatus.Verified;
        if (isImportVerified)
        {
            return;
        }

        var dialog = await DialogService.ShowDialogAsync<CrmVerificationDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        switch (dialogData.ApprovalOpportunity.StatusImport)
        {
            case ImportStatus.Verified:
                await VerifyCrmAsync(dialogData);
                break;
            case ImportStatus.Invalid:
                await DeleteCrmAsync(dialogData);
                break;
            default:
                break;
        }

        // Log.Warning("Import status after: {0}", dialogData.ApprovalOpportunity.StatusImport);
    }

    private async Task VerifyCrmAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been verified";
        await BroadcastService.BroadcastMessageAsync(message);

        IsLoading = false;
    }

    private async Task DeleteCrmAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.DeleteWorkloadAsync(workPaper);

        var message = $"Invalid CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been deleted";
        await BroadcastService.BroadcastMessageAsync(message);

        IsLoading = false;
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.ImportSignaturePx}px
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
            {ColumnWidth.JenisPermohonanPx}px
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

    private static TabNavigation CrmVerificationPage()
    {
        return new TabNavigation("crm-verification", "Verifikasi CRM", PageRoute.CrmImportVerification);
    }
}
