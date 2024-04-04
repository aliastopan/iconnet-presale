namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class MiddleBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnMiddleBoundaryChanged { get; set; }

    public DateTime? NullableMiddleBoundaryDateTimeMin { get; set; }
    public DateTime? NullableMiddleBoundaryDateTimeMax { get; set; }
    public DateTime MiddleBoundaryDateTimeMin => NullableMiddleBoundaryDateTimeMin!.Value;
    public DateTime MiddleBoundaryDateTimeMax => NullableMiddleBoundaryDateTimeMax!.Value;
    public TimeSpan MiddleBoundaryRange => MiddleBoundaryDateTimeMax - MiddleBoundaryDateTimeMin;

    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        NullableMiddleBoundaryDateTimeMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        NullableMiddleBoundaryDateTimeMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
    }

    protected async Task OnMiddleBoundaryDateMinChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        if (nullableDateTime.Value.Date > MiddleBoundaryDateTimeMax.Date)
        {
            LogSwitch.Debug("Invalid middle boundary MIN on MAX. Returning.");
            return;
        }

        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;

        if (nullableDateTime.Value.Date < upperBoundaryMin.Date)
        {
            LogSwitch.Debug("Invalid middle boundary MIN. Set to upper boundary MIN.");
            NullableMiddleBoundaryDateTimeMin = upperBoundaryMin;
        }
        else
        {
            LogSwitch.Debug("Changing middle boundary MIN");
            NullableMiddleBoundaryDateTimeMin = nullableDateTime.Value;
        }

        SessionService.FilterPreference.MiddleBoundaryDateTimeMin = MiddleBoundaryDateTimeMin;

        if (OnMiddleBoundaryChanged.HasDelegate)
        {
            await OnMiddleBoundaryChanged.InvokeAsync();
        }
    }

    protected async Task OnMiddleBoundaryDateMaxChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        if (nullableDateTime.Value.Date < MiddleBoundaryDateTimeMin.Date)
        {
            LogSwitch.Debug("Invalid middle boundary MAX on MIN. Returning.");
            return;
        }

        NullableMiddleBoundaryDateTimeMax = nullableDateTime.Value;
        SessionService.FilterPreference.MiddleBoundaryDateTimeMax = MiddleBoundaryDateTimeMax;

        LogSwitch.Debug("Changing middle boundary MAX");

        if (OnMiddleBoundaryChanged.HasDelegate)
        {
            await OnMiddleBoundaryChanged.InvokeAsync();
        }
    }

    private string GetDaysRangeLabel()
    {
        return $"{MiddleBoundaryRange.Days} Hari";
    }
}
