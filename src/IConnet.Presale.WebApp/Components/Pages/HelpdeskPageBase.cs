using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk page";
    private Guid _sessionId;
    private readonly GridSort<WorkPaper> _sortByStagingStatus = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.HelpdeskInCharge.TglAksi);

    public bool ShowClaims { get; set; } = true;
    public string ToggleText => ShowClaims ? "Hide" : "Show";
    protected int ColumnWidthIdPermohonan { get; set; } = 185;  //px
    protected int ColumnWidthStagingStatus { get; set; } = 90;  //px
    protected int ColumnWidthMax => ColumnWidthIdPermohonan + ColumnWidthStagingStatus;
    protected string IdPermohonanStyle => $"width: {ColumnWidthIdPermohonan}px;";
    protected string StagingStatusStyle => $"width: {ColumnWidthStagingStatus}px;";
    protected string MaxWidthStyle => $"width: {ColumnWidthMax}px;";

    protected override IQueryable<WorkPaper>? WorkPapers => GetMatchInCharge();
    protected GridSort<WorkPaper> SortByStagingStatus => _sortByStagingStatus;
    protected WorkPaper? WorkPaper { get; set; }

    protected string GridTemplateCols
    {
        get => $"{ColumnWidthIdPermohonan}px {ColumnWidthStagingStatus}px";
    }

    protected override async Task OnInitializedAsync()
    {
        _sessionId = await SessionService.GetUserAccountIdAsync();
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyStaged;

        await base.OnInitializedAsync();
    }

    protected IQueryable<WorkPaper>? GetMatchInCharge()
    {
        return base.WorkPapers?.Where(x => x.HelpdeskInCharge.AccountIdSignature == _sessionId);
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

        WorkPaper = row.Item;
        Log.Warning("Selected: {0}", WorkPaper.ApprovalOpportunity.IdPermohonan);

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
            Log.Warning("{0} claim has been removed", dialogData.ApprovalOpportunity.IdPermohonan);

            return;
        }

        await RestageWorkloadAsync(dialogData);
        Log.Warning("{0} claim has been extended", dialogData.ApprovalOpportunity.IdPermohonan);
    }

    private async Task RestageWorkloadAsync(WorkPaper workPaper)
    {
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

    protected bool IsStillInCharge(WorkPaper workPaper, bool debug = false)
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);

        if (debug)
        {
            var timeRemaining = workPaper.HelpdeskInCharge.GetDurationRemaining(now, duration);
            Log.Warning("Time remaining: {0}", timeRemaining);
        }

        return !workPaper.HelpdeskInCharge.IsDurationExceeded(now, duration);
    }
}
