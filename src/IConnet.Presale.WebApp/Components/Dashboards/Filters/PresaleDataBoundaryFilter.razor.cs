namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class PresaleDataBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnUpperBoundaryChanged { get; set; }
    [Parameter] public EventCallback OnReload { get; set; }

    public DateTime? NullableUpperBoundaryDateTimeMin { get; set; }
    public DateTime? NullableUpperBoundaryDateTimeMax { get; set; }
    public DateTime UpperBoundaryDateTimeMin => NullableUpperBoundaryDateTimeMin!.Value;
    public DateTime UpperBoundaryDateTimeMax => NullableUpperBoundaryDateTimeMax!.Value;
    public TimeSpan UpperBoundaryRange => UpperBoundaryDateTimeMax - UpperBoundaryDateTimeMin;

    public DateTime PreviousUpperBoundaryDateTimeMin { get; set; }
    public DateTime PreviousUpperBoundaryDateTimeMax { get; set; }

    public bool IsReloadRequired => !UpperBoundaryUpdateCheck();

    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;

        PreviousUpperBoundaryDateTimeMin = UpperBoundaryDateTimeMin;
        PreviousUpperBoundaryDateTimeMax = UpperBoundaryDateTimeMax;
    }

    protected bool UpperBoundaryUpdateCheck()
    {
        return UpperBoundaryDateTimeMin < PreviousUpperBoundaryDateTimeMin
            || UpperBoundaryDateTimeMax < PreviousUpperBoundaryDateTimeMax;
    }

    protected async Task ReloadAsync()
    {
        PreviousUpperBoundaryDateTimeMin = UpperBoundaryDateTimeMin;
        PreviousUpperBoundaryDateTimeMax = UpperBoundaryDateTimeMax;

        await OnReload.InvokeAsync();
    }

    protected async Task OnUpperBoundaryDateMinChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMin = nullableDateTime.Value;
        SessionService.FilterPreference.UpperBoundaryDateTimeMin = UpperBoundaryDateTimeMin;

        LogSwitch.Debug("Changing upper boundary MIN");

        if (OnUpperBoundaryChanged.HasDelegate)
        {
            await OnUpperBoundaryChanged.InvokeAsync();
        }
    }

    protected async Task OnUpperBoundaryDateMaxChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMax = nullableDateTime.Value;
        SessionService.FilterPreference.UpperBoundaryDateTimeMax = UpperBoundaryDateTimeMax;

        LogSwitch.Debug("Changing upper boundary MAX");

        if (OnUpperBoundaryChanged.HasDelegate)
        {
            await OnUpperBoundaryChanged.InvokeAsync();
        }
    }

    private string GetDaysRangeLabel()
    {
        var today = DateTimeService.DateTimeOffsetNow.Date;
        var isToday = UpperBoundaryDateTimeMax.Date == today;

        return isToday
            ? $"{UpperBoundaryRange.Days} Hari Terakhir"
            : $"Rentang {UpperBoundaryRange.Days} Hari";
    }
}
