using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class MiddleBoundaryFilter : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    public DateTime CurrentMiddleBoundaryDateTimeMin { get; set; }
    public DateTime CurrentMiddleBoundaryDateTimeMax { get; set; }

    public DateTime? NullableMiddleBoundaryDateTimeMin { get; set; }
    public DateTime? NullableMiddleBoundaryDateTimeMax { get; set; }
    public DateTime MiddleBoundaryDateTimeMin => NullableMiddleBoundaryDateTimeMin!.Value;
    public DateTime MiddleBoundaryDateTimeMax => NullableMiddleBoundaryDateTimeMax!.Value;
    public TimeSpan MiddleBoundaryRange => MiddleBoundaryDateTimeMax - MiddleBoundaryDateTimeMin;

    public string MiddleBoundaryTimeRangeLabel => GetMiddleBoundaryDaysRangeLabel();

    protected override void OnInitialized()
    {
        NullableMiddleBoundaryDateTimeMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        NullableMiddleBoundaryDateTimeMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;

        CurrentMiddleBoundaryDateTimeMin = new DateTime(MiddleBoundaryDateTimeMin.Ticks);
        CurrentMiddleBoundaryDateTimeMax = new DateTime(MiddleBoundaryDateTimeMax.Ticks);
    }

    protected string GetCurrentMiddleBoundaryDateMin()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentMiddleBoundaryDateTimeMin.ToString("dd MMM yyyy", cultureInfo);
    }

    protected string GetCurrentMiddleBoundaryDateMax()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentMiddleBoundaryDateTimeMax.ToString("dd MMM yyyy", cultureInfo);
    }

    protected void OnMiddleBoundaryDateMinChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        bool isOutOfRange = nullableDateTime.Value.Date > MiddleBoundaryDateTimeMax.Date;

        if (isOutOfRange)
        {
            LogSwitch.Debug("Invalid middle boundary out of range.");
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
    }

    protected void OnMiddleBoundaryDateMaxChangedAsync(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        bool isOutOfRange = nullableDateTime.Value.Date < MiddleBoundaryDateTimeMin.Date;

        if (isOutOfRange)
        {
            LogSwitch.Debug("Invalid middle boundary out of range.");
            return;
        }

        NullableMiddleBoundaryDateTimeMax = nullableDateTime.Value;
        SessionService.FilterPreference.MiddleBoundaryDateTimeMax = MiddleBoundaryDateTimeMax;

        LogSwitch.Debug("Changing middle boundary MAX");
    }

    private string GetMiddleBoundaryDaysRangeLabel()
    {
        return $"Rentang {MiddleBoundaryRange.Days} Hari";
    }
}
