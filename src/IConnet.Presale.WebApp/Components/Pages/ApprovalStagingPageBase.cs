using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;

namespace IConnet.Presale.WebApp.Components.Pages;

public class ApprovalStagingPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly static int _stagingLimit = 25;

    protected string GridTemplateCols => GetGridTemplateCols();
    protected FilterForm FilterComponent { get; set; } = default!;
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("approval-staging", PageNavName.ApprovalStaging, PageRoute.ApprovalStaging);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.ApprovalStaging;
        WorkloadFilter = WorkloadFilter.OnlyWaitingApproval;

        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
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

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.ApprovalDuration;
        var timeRemaining = workPaper!.SignaturePlanningAssetCoverageInCharge.GetDurationRemaining(now, duration);

        var isNotStaged = workPaper!.SignaturePlanningAssetCoverageInCharge.IsEmptySignature();
        var hasStageExpired = workPaper!.SignaturePlanningAssetCoverageInCharge.IsDurationExceeded(now, duration);
        var isOnGoingApproval = workPaper!.ProsesApproval.IsOnGoing();

        if ((isNotStaged || hasStageExpired) && isOnGoingApproval)
        {
            await OpenStagingDialogAsync(row.Item);
        }
        else
        {
            if (!isOnGoingApproval)
            {
                DoneProcessingToast();
                return;
            }

            await OnGoingApprovalToastAsync(workPaper!.SignaturePlanningAssetCoverageInCharge.Alias);
        }
    }

    protected async Task OnGoingApprovalToastAsync(string inChargeAlias)
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
            Title = "Tampung Kertas Kerja (Approval)",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ApprovalStagingDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        await StageWorkPaperAsync(dialogData);
    }

    protected async Task StageWorkPaperAsync(WorkPaper workPaper)
    {
        var count = await GetStageCountAsync();
        if (count > _stagingLimit)
        {
            workPaper.SetPlanningAssetCoverageInCharge(RevertStagingSignature());
            StagingReachLimitToast();

            return;
        }

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"PAC {workPaper.SignaturePlanningAssetCoverageInCharge.Alias} has staged '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    protected bool IsClosedLost(ValidationProcess validationProcess)
    {
        DateTime today = DateTimeService.DateTimeOffsetNow.Date;

        return validationProcess.IsClosedLost(today);
    }

    private async Task<int> GetStageCountAsync()
    {
        var alias = await SessionService.GetSessionAliasAsync();
        var count = WorkPapers!.Where(x => x.SignaturePlanningAssetCoverageInCharge.Alias == alias).Count();

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
            {ColumnWidth.IdPermohonanPx + ColumnWidth.OffsetPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.StatusApprovalPx}px
            {ColumnWidth.PlanningAssetCoverageInChargePx}px
            {ColumnWidth.ValidasiNamaPelangganPx}px
            {ColumnWidth.ValidasiNomorTelpPx}px
            {ColumnWidth.ValidasiEmailPx}px
            {ColumnWidth.ValidasiIdPlnPx}px
            {ColumnWidth.ValidasiAlamatPx}px
            {ColumnWidth.ValidasiShareLocPx}px
            {ColumnWidth.LinkChatHistoryPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.KantorPerwakilanPx}px";
    }
}
