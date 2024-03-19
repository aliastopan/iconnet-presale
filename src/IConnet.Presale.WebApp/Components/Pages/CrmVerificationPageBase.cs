using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmVerificationPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;
    [Inject] public SqlPushService SqlPushService { get; init; } = default!;

    protected FilterForm FilterComponent { get; set; } = default!;
    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    protected GridSort<WorkPaper> SortByImportSignatureAlias => WorkPapers.SortByImportSignatureAlias();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("crm-verification", PageNavName.CrmVerification, PageRoute.CrmVerification);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.CrmVerification;
        WorkloadFilter = WorkloadFilter.OnlyImportUnverified;

        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ColumnWidth.SetColumnWidth(WorkPapers);
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        if (row.Item.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature())
        {
            await BeginVerification(row);
            return;
        }

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.VerificationDuration;
        var alias = row.Item.ApprovalOpportunity.SignatureVerifikasiImport.Alias;
        var idPermohonan = row.Item.ApprovalOpportunity.IdPermohonan;
        var isLockExpired = row.Item.ApprovalOpportunity.SignatureVerifikasiImport.IsDurationExceeded(now, duration);

        if (isLockExpired)
        {
            await BeginVerification(row);
            return;
        }

        if (await SessionService.IsAliasMatch(alias))
        {
            await BeginVerification(row);
            return;
        }

        ConcurrencyLockedToast(idPermohonan, alias);

        async Task BeginVerification(FluentDataGridRow<WorkPaper> row)
        {
            row.Item!.ApprovalOpportunity.SignatureVerifikasiImport = await SessionService.GenerateActionSignatureAsync();

            var broadcastMessage = $"Began '{row.Item!.ApprovalOpportunity.IdPermohonan}' verification";
            await BroadcastService.BroadcastMessageAsync(broadcastMessage);

            await OpenVerificationDialogAsync(row.Item);
        }
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

    protected async Task OpenVerificationDialogAsync(WorkPaper workPaper)
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

        if (result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;

        if (result.Cancelled)
        {
            dialogData.ApprovalOpportunity.SignatureVerifikasiImport = ActionSignature.Empty();

            CancelVerificationToast(dialogData.ApprovalOpportunity.IdPermohonan);

            var broadcastMessage = $"Cancel '{dialogData.ApprovalOpportunity.IdPermohonan}' verification";
            await BroadcastService.BroadcastMessageAsync(broadcastMessage);

            LogSwitch.Debug("{0} verification has been canceled", dialogData.ApprovalOpportunity.IdPermohonan);

            return;
        }

        switch (dialogData.ApprovalOpportunity.StatusImport)
        {
            case ImportStatus.Verified:
                await VerifyCrmAsync(dialogData);
                break;
            case ImportStatus.Invalid:
                await MarkCrmInvalidAsync(dialogData);
                break;
            default:
                break;
        }
    }

    private async Task VerifyCrmAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been verified";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

        IsLoading = false;
    }

    private async Task MarkCrmInvalidAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been mark as invalid";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

        await SqlPushService.SqlPushAsync(workPaper);

        IsLoading = false;
    }

    private void ConcurrencyLockedToast(string idPermohonan, string alias)
    {
        var intent = ToastIntent.Warning;
        var message = $"Data CRM {idPermohonan} sedang diverifikasi oleh {alias}";

        ToastService.ShowToast(intent, message);
    }

    private void CancelVerificationToast(string idPermohonan)
    {
        var intent = ToastIntent.Warning;
        var message = $"Data CRM {idPermohonan} telah batal diverifikasi";
        var timeout = 3000;

        ToastService.ShowToast(intent, message, timeout);
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.InChargeImportPx}px
            {ColumnWidth.InChargeVerificationPx}px
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
}
