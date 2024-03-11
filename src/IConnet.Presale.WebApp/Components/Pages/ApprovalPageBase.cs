using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class ApprovalPageBase : WorkloadPageBase, IPageNavigation
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private Guid _sessionId;
    private readonly List<WorkPaperApprovalModel> _approvalModels = [];
    private readonly GridSort<WorkPaper> _sortByStagingStatus = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.SignaturePlanningAssetCoverageInCharge.TglAksi);

    protected string GridTemplateCols => GetGridTemplateCols();
    protected FilterForm FilterComponent { get; set; } = default!;
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    protected GridSort<WorkPaper> SortByStagingStatus => _sortByStagingStatus;
    protected WorkPaper? ActiveWorkPaper { get; set; }
    protected WorkPaperApprovalModel? ActiveApprovalModel { get; set; }

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("approval", PageNavName.Approval, PageRoute.Approval);
    }

    protected override void OnInitialized()
    {
        PageName = PageNavName.Approval;
        WorkloadFilter = WorkloadFilter.OnlyWaitingApproval;

        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        _sessionId = await SessionService.GetUserAccountIdAsync();

        await base.OnInitializedAsync();

        if (WorkPapers is null)
        {
            return;
        }

        foreach (var workPaper in WorkPapers)
        {
            _approvalModels.Add(new WorkPaperApprovalModel(workPaper));
        }

        // LogSwitch.Debug("Approval Models {count}", _approvalModels.Count);
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        IQueryable<WorkPaper>? workPapers = FilterComponent.FilterWorkPapers(base.WorkPapers)?
            .Where(x => x.SignaturePlanningAssetCoverageInCharge.AccountIdSignature == _sessionId
                && x.ProsesApproval.IsOnGoing());

        ColumnWidth.SetColumnWidth(workPapers);
        return workPapers;
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        if (!IsStillInCharge(row.Item))
        {
            await OpenDialogAsync(row.Item);
            return;
        }

        ActiveWorkPaper = row.Item;
        ActiveApprovalModel = _approvalModels
            .FirstOrDefault(x => x.IdPermohonan == row.Item.ApprovalOpportunity.IdPermohonan);

        // LogSwitch.Debug("Selected: {0}", WorkPaper.ApprovalOpportunity.IdPermohonan);
    }

    public void DeselectWorkPaper()
    {
        // LogSwitch.Debug("Deselected: {0}", ActiveWorkPaper!.ApprovalOpportunity.IdPermohonan);

        ActiveWorkPaper = null;
        ActiveApprovalModel = null;
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Tampung Kertas Kerja",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<ApprovalStagingAlertDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        if (dialogData.SignaturePlanningAssetCoverageInCharge.IsEmptySignature())
        {
            await UnstageWorkPaperAsync(dialogData);

            return;
        }

        await RestageWorkPaperAsync(dialogData);
    }

    protected async Task ScrollToApprovalForm()
    {
        var elementId = "approval-id";
        await JsRuntime.InvokeVoidAsync("scrollToElement", elementId);

        // LogSwitch.Debug("Scrolling...");
    }

    protected bool IsStillInCharge(WorkPaper workPaper, bool debug = false)
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = InChargeDuration.ApprovalDuration;

        if (debug)
        {
            var timeRemaining = workPaper.SignaturePlanningAssetCoverageInCharge.GetDurationRemaining(now, duration);
            LogSwitch.Debug("Time remaining: {0}", timeRemaining);
        }

        return !workPaper.SignaturePlanningAssetCoverageInCharge.IsDurationExceeded(now, duration);
    }

    private async Task RestageWorkPaperAsync(WorkPaper workPaper)
    {
        ActiveWorkPaper = workPaper;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' staging claim has been extended";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private async Task UnstageWorkPaperAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' is no longer staged";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx + ColumnWidth.OffsetPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.StagingStatusPx}px
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
