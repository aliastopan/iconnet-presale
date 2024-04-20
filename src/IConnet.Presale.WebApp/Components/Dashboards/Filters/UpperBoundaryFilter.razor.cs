using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class UpperBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    public DateTime CurrentUpperBoundaryDateTimeMin { get; set; }
    public DateTime CurrentUpperBoundaryDateTimeMax { get; set; }

    public DateTime? NullableUpperBoundaryDateTimeMin { get; set; }
    public DateTime? NullableUpperBoundaryDateTimeMax { get; set; }
    public DateTime UpperBoundaryDateTimeMin => NullableUpperBoundaryDateTimeMin!.Value;
    public DateTime UpperBoundaryDateTimeMax => NullableUpperBoundaryDateTimeMax!.Value;
    public TimeSpan UpperBoundaryRange => UpperBoundaryDateTimeMax - UpperBoundaryDateTimeMin;

    public string UpperBoundaryTimeRangeLabel => GetUpperBoundaryDaysRangeLabel();

    protected override void OnInitialized()
    {
        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;

        CurrentUpperBoundaryDateTimeMin = new DateTime(UpperBoundaryDateTimeMin.Ticks);
        CurrentUpperBoundaryDateTimeMax = new DateTime(UpperBoundaryDateTimeMax.Ticks);
    }

    protected string GetCurrentUpperBoundaryDateMin()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentUpperBoundaryDateTimeMin.ToString("dd MMM yyyy", cultureInfo);
    }

    protected string GetCurrentUpperBoundaryDateMax()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentUpperBoundaryDateTimeMax.ToString("dd MMM yyyy", cultureInfo);
    }

    protected void OnUpperBoundaryDateMinChanged(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMin = nullableDateTime.Value;
        SessionService.FilterPreference.UpperBoundaryDateTimeMin = UpperBoundaryDateTimeMin;
    }

    protected void OnUpperBoundaryDateMaxChanged(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        NullableUpperBoundaryDateTimeMax = nullableDateTime.Value;
        SessionService.FilterPreference.UpperBoundaryDateTimeMax = UpperBoundaryDateTimeMax;

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
