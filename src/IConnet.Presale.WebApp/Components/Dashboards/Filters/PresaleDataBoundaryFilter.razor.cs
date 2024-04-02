namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class PresaleDataBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    private readonly int _filterDaysRangeDefault = 10; // 31

    public DateTime? NullableFilterDateTimeMin { get; set; }
    public DateTime? NullableFilterDateTimeMax { get; set; }
    public DateTime FilterDateTimeMin => NullableFilterDateTimeMin!.Value;
    public DateTime FilterDateTimeMax => NullableFilterDateTimeMax!.Value;
    public TimeSpan FilterDateTimeDifference => FilterDateTimeMax - FilterDateTimeMin;

    public string FilterDateTimeRangeLabel => GetDaysRangeLabel();

    protected override void OnInitialized()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;

        NullableFilterDateTimeMin = today.AddDays(-_filterDaysRangeDefault);
        NullableFilterDateTimeMax = today;
    }

    protected void SetFilterDateTimeMin(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableFilterDateTimeMin = nullableDateTime.Value;
    }

    protected void SetFilterDateTimeMax(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableFilterDateTimeMax = nullableDateTime.Value;
    }

    private string GetDaysRangeLabel()
    {
        var today = DateTimeService.DateTimeOffsetNow.Date;
        var isToday = FilterDateTimeMax.Date == today;

        return isToday
            ? $"{FilterDateTimeDifference.Days} Hari Terakhir"
            : $"Rentang {FilterDateTimeDifference.Days} Hari";
    }
}
