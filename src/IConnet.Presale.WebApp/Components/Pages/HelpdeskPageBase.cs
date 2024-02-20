using IConnet.Presale.WebApp.Components.Dialogs;
using IConnet.Presale.WebApp.Components.Forms;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk page";
    private Guid _sessionId;
    private readonly List<WorkloadValidationModel> _validationModels = new List<WorkloadValidationModel>();
    private readonly GridSort<WorkPaper> _sortByStagingStatus = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.HelpdeskInCharge.TglAksi);

    // TODO: create helper class
    public bool ShowStagingClaims { get; set; } = true;
    public string ToggleText => ShowStagingClaims ? "Hide" : "Show";
    protected int ColumnWidthIdPermohonan { get; set; } = 185;  //px
    protected int ColumnWidthStagingStatus { get; set; } = 90;  //px
    protected int ColumnWidthMax => ColumnWidthIdPermohonan + ColumnWidthStagingStatus;
    protected string IdPermohonanStyle => $"width: {ColumnWidthIdPermohonan}px;";
    protected string StagingStatusStyle => $"width: {ColumnWidthStagingStatus}px;";
    protected string MaxWidthStyle => ShowStagingClaims ? $"width: {ColumnWidthMax}px;" : "";

    protected string GridTemplateCols => GetGridTemplateCols();
    protected FilterForm FilterComponent { get; set; } = default!;
    protected override IQueryable<WorkPaper>? WorkPapers => FilterWorkPapers();

    protected GridSort<WorkPaper> SortByStagingStatus => _sortByStagingStatus;
    protected WorkPaper? ActiveWorkPaper { get; set; }
    protected WorkloadValidationModel? ActiveValidationModel => _validationModels
        .FirstOrDefault(x => x.IdPermohonan == ActiveWorkPaper?.ApprovalOpportunity.IdPermohonan);

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(HelpdeskPage());

        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        _sessionId = await SessionService.GetUserAccountIdAsync();
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyStaged;

        await base.OnInitializedAsync();

        if (WorkPapers is null)
        {
            return;
        }

        foreach (var workPaper in WorkPapers)
        {
            _validationModels.Add(new WorkloadValidationModel(workPaper));
        }

        LogSwitch.Debug("Validation Models {count}", _validationModels.Count);
    }

    protected IQueryable<WorkPaper>? FilterWorkPapers()
    {
        if (FilterComponent is null)
        {
            return base.WorkPapers;
        }

        IQueryable<WorkPaper>? workPapers = FilterComponent.FilterWorkPapers(base.WorkPapers)?
            .Where(x => x.HelpdeskInCharge.AccountIdSignature == _sessionId);

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
        ActiveValidationModel!.NullableTanggalRespons = DateTimeService.DateTimeOffsetNow.DateTime;
        ActiveValidationModel!.NullableWaktuRespons = DateTimeService.DateTimeOffsetNow.DateTime;
        // Log.Warning("Selected: {0}", WorkPaper.ApprovalOpportunity.IdPermohonan);

    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Tampung Kertas Kerja",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<WorkloadStagingAlertDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        if (dialogData.HelpdeskInCharge.IsEmptySignature())
        {
            await UnstageWorkloadAsync(dialogData);
            // Log.Warning("{0} claim has been removed", dialogData.ApprovalOpportunity.IdPermohonan);

            return;
        }

        await RestageWorkloadAsync(dialogData);
        // Log.Warning("{0} claim has been extended", dialogData.ApprovalOpportunity.IdPermohonan);
    }

    protected bool IsStillInCharge(WorkPaper workPaper, bool debug = false)
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);

        if (debug)
        {
            var timeRemaining = workPaper.HelpdeskInCharge.GetDurationRemaining(now, duration);
            // Log.Warning("Time remaining: {0}", timeRemaining);
        }

        return !workPaper.HelpdeskInCharge.IsDurationExceeded(now, duration);
    }

    private async Task RestageWorkloadAsync(WorkPaper workPaper)
    {
        ActiveWorkPaper = workPaper;
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' staging claim has been extended";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private async Task UnstageWorkloadAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"Workload '{workPaper.ApprovalOpportunity.IdPermohonan}' is no longer staged";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.StagingStatusPx}px
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
            {ColumnWidth.StatusValidasiPx}px
            {ColumnWidth.KeteranganValidasiPx}px
            {ColumnWidth.ContactWhatsAppPx}px
            {ColumnWidth.KantorPerwakilanPx}px";
    }

    private static TabNavigation HelpdeskPage()
    {
        return new TabNavigation("helpdesk", "Helpdesk", PageRoute.Helpdesk);
    }
}
