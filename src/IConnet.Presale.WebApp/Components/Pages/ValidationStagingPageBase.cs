using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class ValidationStagingPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk staging page";
    private readonly static int _stagingLimit = 10;

    protected string GridTemplateCols => GetGridTemplateCols();
    protected FilterForm FilterComponent { get; set; } = default!;
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(ValidationStagingPage());

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyValidating;

        await base.OnInitializedAsync();

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

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        var workPaper = row.Item;
        // Log.Warning("Selected row {0}", workPaper is null ? "null" : workPaper.ApprovalOpportunity.IdPermohonan);

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);
        var timeRemaining = workPaper!.SignatureHelpdeskInCharge.GetDurationRemaining(now, duration);
        var label = timeRemaining > TimeSpan.Zero ? "Active" : "Expired";

        // Log.Warning("Time remaining: {0} {1}", timeRemaining, label);

        var isNotStaged = workPaper!.SignatureHelpdeskInCharge.IsEmptySignature();
        var hasStageExpired = workPaper!.SignatureHelpdeskInCharge.IsDurationExceeded(now, duration);
        var isOnGoingValidation = workPaper!.ProsesValidasi.IsOnGoing;

        if ((isNotStaged || hasStageExpired) && isOnGoingValidation)
        {
            await OpenStagingDialogAsync(row.Item);
        }
        else
        {
            if (!isOnGoingValidation)
            {
                DoneProcessingToast();
                return;
            }

            await OnGoingValidationToastAsync(workPaper!.SignatureHelpdeskInCharge.Alias);
        }
    }

    protected async Task OnGoingValidationToastAsync(string inChargeAlias)
    {
        var intent = ToastIntent.Warning;
        var message = await SessionService.IsAliasMatch(inChargeAlias)
            ? "Presale telah Anda tampung."
            : $"Presale masih diproses oleh {inChargeAlias}.";

        ToastService.ShowToast(intent, message);
    }

    protected void StagingReachLimitToast()
    {
        var intent = ToastIntent.Error;
        var message = $"Jumlah tampungan Kertas Kerja ({_stagingLimit}) telah melebihi batas.";
        ToastService.ShowToast(intent, message);
    }

    protected void DoneProcessingToast()
    {
        var intent = ToastIntent.Info;
        var message = $"Kertas Kerja telah selesai diproses.";
        ToastService.ShowToast(intent, message);
    }

    protected async Task OpenStagingDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Tampung Kertas Kerja",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ValidationStagingDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        await StageWorkloadAsync(dialogData);
    }

    private async Task StageWorkloadAsync(WorkPaper workPaper)
    {
        var count = await GetStageCountAsync();
        if (count > _stagingLimit)
        {
            workPaper.SignatureHelpdeskInCharge = RevertStagingSignature();
            StagingReachLimitToast();

            return;
        }

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"{workPaper.SignatureHelpdeskInCharge.Alias} has staged '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private async Task<int> GetStageCountAsync()
    {
        var alias = await SessionService.GetSessionAliasAsync();
        var count = WorkPapers!.Where(x => x.SignatureHelpdeskInCharge.Alias == alias).Count();

        // Log.Warning("Current staging count {0}", count);
        return count;
    }

    private ActionSignature RevertStagingSignature()
    {
        return new ActionSignature
        {
            AccountIdSignature = Guid.Empty,
            Alias = string.Empty,
            TglAksi = DateTimeService.Zero
        };
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.HelpdeskInChargePx}px
            {ColumnWidth.ShiftPx}px
            {ColumnWidth.TglChatCallMulaiPx}px
            {ColumnWidth.ValidasiNamaPelangganPx}px
            {ColumnWidth.ValidasiNomorTelpPx}px
            {ColumnWidth.ValidasiEmailPx}px
            {ColumnWidth.ValidasiIdPlnPx}px
            {ColumnWidth.ValidasiAlamatPx}px
            {ColumnWidth.ValidasiShareLocPx}px
            {ColumnWidth.TglChatCallResponsPx}px
            {ColumnWidth.LinkRekapChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.KantorPerwakilanPx}px";
    }

    private static TabNavigationModel ValidationStagingPage()
    {
        return new TabNavigationModel("validation-staging", PageNavName.ValidationStaging, PageRoute.ValidationStaging);
    }
}
