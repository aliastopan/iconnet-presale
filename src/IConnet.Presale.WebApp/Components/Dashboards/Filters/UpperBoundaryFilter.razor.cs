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
        CurrentUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        CurrentUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;

        NullableUpperBoundaryDateTimeMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        NullableUpperBoundaryDateTimeMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
    }

    protected string GetCurrentMin()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentUpperBoundaryDateTimeMin.ToString("dd MMM yyyy", cultureInfo);
    }

    protected string GetCurrentMax()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentUpperBoundaryDateTimeMax.ToString("dd MMM yyyy", cultureInfo);
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

        await Task.CompletedTask;
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

        await Task.CompletedTask;
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
