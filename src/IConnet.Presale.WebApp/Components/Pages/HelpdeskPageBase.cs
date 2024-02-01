namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk page";
    private Guid _sessionId;

    public bool ShowClaims { get; set; } = true;
    public string ToggleText => ShowClaims ? "Hide" : "Show";

    protected override IQueryable<WorkPaper>? WorkPapers => GetMatchInCharge();

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

    protected bool IsStillInCharge(WorkPaper workPaper)
    {
        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);

        return !workPaper.HelpdeskInCharge.IsDurationExceeded(now, duration);
    }
}
