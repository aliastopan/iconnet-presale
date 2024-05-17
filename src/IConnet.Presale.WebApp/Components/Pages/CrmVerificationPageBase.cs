using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmVerificationPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public SqlPushService SqlPushService { get; init; } = default!;

    private bool _firstLoad = true;
    private ActionSignature _persistentPicVerification = default!;
    private IQueryable<WorkPaper>? _filteredWorkPapers;

    protected string GridTemplateCols => GetGridTemplateCols();
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("crm-verification", PageNavName.CrmVerification, PageRoute.CrmVerification);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.CrmVerification;
        PresaleDataFilter = PresaleDataFilter.OnlyImportUnverified;

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

        _persistentPicVerification = new ActionSignature(row.Item.ApprovalOpportunity.SignatureVerifikasiImport);
        // LogSwitch.Debug("PIC Persistent. {0}", _persistentPicVerification.Alias);

        if (row.Item.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature())
        {
            await BeginVerification(row);
            return;
        }

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.VerificationDuration;
        // var duration = new TimeSpan(0, 0, 15);
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

            await WorkloadManager.UpdateWorkloadAsync(row.Item);

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

    protected async Task OpenVerificationDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Verifikasi Import CRM",
            TrapFocus = true,
            PreventDismissOnOverlayClick = true,
            Width = "650px",
        };

        var isImportVerified = workPaper.ApprovalOpportunity.StatusImport == ImportStatus.Verified;
        var IsReinstated = workPaper.WorkPaperLevel == WorkPaperLevel.Reinstated;

        if (isImportVerified && !IsReinstated)
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
            if (!dialogData.OnWait)
            {
                // LogSwitch.Debug("NOT on wait. Resetting Signature");
                dialogData.ApprovalOpportunity.SignatureVerifikasiImport = ActionSignature.Empty();
            }
            else
            {
                // LogSwitch.Debug("ON WAIT. Restoring Signature");
                dialogData.ApprovalOpportunity.SignatureVerifikasiImport = _persistentPicVerification;
            }

            await WorkloadManager.UpdateWorkloadAsync(dialogData);

            CancelVerificationToast(dialogData.ApprovalOpportunity.IdPermohonan);

            var broadcastMessage = $"Cancel '{dialogData.ApprovalOpportunity.IdPermohonan}' verification";
            await BroadcastService.BroadcastMessageAsync(broadcastMessage);

            return;
        }

        if (dialogData.OnWait)
        {
            await PutOnWaitAsync(dialogData);
            ColumnWidth.SetColumnWidth(WorkPapers);
        }

        switch (dialogData.ApprovalOpportunity.StatusImport)
        {
            case ImportStatus.Verified:
                {
                    bool isDoneProcessing = dialogData.WorkPaperLevel == WorkPaperLevel.DoneProcessing;
                    bool isDirectlyApproved = !dialogData.ProsesApproval.DirectApproval.IsNullOrWhiteSpace();

                    if (isDoneProcessing && isDirectlyApproved)
                    {
                        await DirectApprovalAsync(dialogData);
                    }
                    else
                    {
                        await VerifyCrmAsync(dialogData);
                    }
                }
                break;
            case ImportStatus.Invalid:
                await MarkCrmInvalidAsync(dialogData);
                break;
            case ImportStatus.ToBeDeleted:
                await DeleteCrmAsync(dialogData);
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

    private async Task PutOnWaitAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been put on wait";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

        IsLoading = false;
    }

    private async Task DirectApprovalAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been directly approved";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

        SqlPushService.SqlPush(workPaper);

        IsLoading = false;
    }

    private async Task MarkCrmInvalidAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var broadcastMessage = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been mark as invalid";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

        SqlPushService.SqlPush(workPaper);

        IsLoading = false;
    }

    private async Task DeleteCrmAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.DeleteWorkloadAsync(workPaper);

        var broadcastMessage = $"Invalid CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been deleted";
        await BroadcastService.BroadcastMessageAsync(broadcastMessage);

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
            {ColumnWidth.KeteranganVerifikasiPx}px
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
