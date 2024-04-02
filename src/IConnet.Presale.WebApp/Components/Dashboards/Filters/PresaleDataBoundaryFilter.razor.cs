namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class PresaleDataBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnUpperBoundaryChanged { get; set; }

    public DateTime? NullableUpperBoundaryDateTimeMin { get; set; }
    public DateTime? NullableUpperBoundaryDateTimeMax { get; set; }
    public DateTime UpperBoundaryDateTimeMin => NullableUpperBoundaryDateTimeMin!.Value;
    public DateTime UpperBoundaryDateTimeMax => NullableUpperBoundaryDateTimeMax!.Value;
    public TimeSpan UpperBoundaryRange => UpperBoundaryDateTimeMax - UpperBoundaryDateTimeMin;

    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;

        SessionService.FilterPreference.SetBoundaryDateTimeDefault(today);

        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
    }

    protected async Task OnUpperBoundaryDateMinChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMin = nullableDateTime.Value;

        LogSwitch.Debug("Changing upper boundary MIN");

        await OnUpperBoundaryChanged.InvokeAsync();
    }

    protected async Task OnUpperBoundaryDateMaxChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMax = nullableDateTime.Value;

        LogSwitch.Debug("Changing upper boundary MAX");

        await OnUpperBoundaryChanged.InvokeAsync();
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
