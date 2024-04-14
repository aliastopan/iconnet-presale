namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class PresaleDataBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnUpperBoundaryChanged { get; set; }
    [Parameter] public EventCallback OnMiddleBoundaryChanged { get; set; }
    [Parameter] public EventCallback OnLowerBoundaryChanged { get; set; }
    [Parameter] public EventCallback OnReload { get; set; }

    public DateTime? NullableUpperBoundaryDateTimeMin { get; set; }
    public DateTime? NullableUpperBoundaryDateTimeMax { get; set; }
    public DateTime UpperBoundaryDateTimeMin => NullableUpperBoundaryDateTimeMin!.Value;
    public DateTime UpperBoundaryDateTimeMax => NullableUpperBoundaryDateTimeMax!.Value;
    public TimeSpan UpperBoundaryRange => UpperBoundaryDateTimeMax - UpperBoundaryDateTimeMin;
    public DateTime PreviousUpperBoundaryDateTimeMin { get; set; }
    public DateTime PreviousUpperBoundaryDateTimeMax { get; set; }

    public DateTime? NullableMiddleBoundaryDateTimeMin { get; set; }
    public DateTime? NullableMiddleBoundaryDateTimeMax { get; set; }
    public DateTime MiddleBoundaryDateTimeMin => NullableMiddleBoundaryDateTimeMin!.Value;
    public DateTime MiddleBoundaryDateTimeMax => NullableMiddleBoundaryDateTimeMax!.Value;
    public TimeSpan MiddleUpperBoundaryRange => MiddleBoundaryDateTimeMax - MiddleBoundaryDateTimeMin;

    public DateTime? NullableLowerBoundaryDateTime { get; set; }
    public DateTime LowerBoundaryDateTime => NullableLowerBoundaryDateTime!.Value;

    public bool IsReloadRequired => !UpperBoundaryUpdateCheck();

    protected bool IsMonthlyView => SessionService.FilterPreference.IsMonthlySelected;
    protected bool IsWeeklyView => SessionService.FilterPreference.IsWeeklySelected;
    protected bool IsDailyView => SessionService.FilterPreference.IsDailySelected;

    public string UpperBoundaryTimeRangeLabel => GetUpperBoundaryDaysRangeLabel();

    protected override void OnInitialized()
    {
        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
        NullableMiddleBoundaryDateTimeMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        NullableMiddleBoundaryDateTimeMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
        NullableLowerBoundaryDateTime = SessionService.FilterPreference.LowerBoundaryDateTime;

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

    protected async Task OnMiddleBoundaryDateMinChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableMiddleBoundaryDateTimeMin = nullableDateTime.Value;
        SessionService.FilterPreference.MiddleBoundaryDateTimeMin = MiddleBoundaryDateTimeMin;

        LogSwitch.Debug("Changing middle boundary MIN");

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

        NullableMiddleBoundaryDateTimeMax = nullableDateTime.Value;
        SessionService.FilterPreference.MiddleBoundaryDateTimeMax = MiddleBoundaryDateTimeMax;

        LogSwitch.Debug("Changing middle boundary MAX");

        if (OnMiddleBoundaryChanged.HasDelegate)
        {
            await OnMiddleBoundaryChanged.InvokeAsync();
        }
    }

    protected async Task OnLowerBoundaryDateChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableLowerBoundaryDateTime = nullableDateTime.Value;
        SessionService.FilterPreference.LowerBoundaryDateTime = LowerBoundaryDateTime;

        LogSwitch.Debug("Changing lower boundary");

        if (OnLowerBoundaryChanged.HasDelegate)
        {
            await OnLowerBoundaryChanged.InvokeAsync();
        }
    }

    private string GetUpperBoundaryDaysRangeLabel()
    {
        var today = DateTimeService.DateTimeOffsetNow.Date;
        var isToday = UpperBoundaryDateTimeMax.Date == today;

        return isToday
            ? $"{UpperBoundaryRange.Days} Hari Terakhir"
            : $"Rentang {UpperBoundaryRange.Days} Hari";
    }
}
