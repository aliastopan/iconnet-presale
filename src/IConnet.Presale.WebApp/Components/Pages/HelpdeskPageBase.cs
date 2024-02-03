using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk page";
    private Guid _sessionId;

    public bool ShowClaims { get; set; } = true;
    public string ToggleText => ShowClaims ? "Hide" : "Show";

    protected override IQueryable<WorkPaper>? WorkPapers => GetMatchInCharge();
    protected WorkPaper? WorkPaper { get; set; }

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
