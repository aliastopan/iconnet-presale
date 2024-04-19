using System.Globalization;
using Microsoft.AspNetCore.Components.Web;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ReportTabulationStackBase : ComponentBase
{
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    [Parameter] public string TabulationId { get; set; } = default!;
    [Parameter] public EventCallback PageRefresh { get; set; }
    [Parameter] public EventCallback OpenBoundaryFilter { get; set; }

    private CultureInfo _cultureInfo = new CultureInfo("id-ID");

    public bool IsMonthlySelected => SessionService.FilterPreference.IsMonthlySelected;
    public bool IsWeeklySelected => SessionService.FilterPreference.IsWeeklySelected;
    public bool IsDailySelected => SessionService.FilterPreference.IsDailySelected;
    public bool IsBufferLoading => SessionService.FilterPreference.IsBufferLoading;

    protected Appearance MonthlyToggleAppearance => IsMonthlySelected ? Appearance.Accent : Appearance.Neutral;
    protected Appearance WeeklyToggleAppearance => IsWeeklySelected ? Appearance.Accent : Appearance.Neutral;
    protected Appearance DailyToggleAppearance => IsDailySelected ? Appearance.Accent : Appearance.Neutral;

    protected string MonthlyToggleDisplayStyle => IsMonthlySelected ? "d-flex flex-column w-100" : "display: none";
    protected string WeeklyToggleDisplayStyle => IsWeeklySelected ? "d-flex flex-column w-100" : "display: none";
    protected string DailyToggleDisplayStyle => IsDailySelected ? "d-flex flex-column w-100" : "display: none";

    protected override void OnInitialized()
    {
        SessionService.FilterPreference.ToggleToMonthlyView();
    }

    public async Task OpenBoundaryFilterAsync()
    {
        if (OpenBoundaryFilter.HasDelegate)
        {
            LogSwitch.Debug("Tabulation Stack calling Boundary Filtering");

            await OpenBoundaryFilter.InvokeAsync();
        }
    }

    protected string GetUpperBoundaryDateMin()
    {
        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;

        return upperBoundaryMin.ToString("dd MMM yyyy", _cultureInfo);
    }

    protected string GetUpperBoundaryDateMax()
    {
        var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;

        return upperBoundaryMax.ToString("dd MMM yyyy", _cultureInfo);
    }

    protected string GetMiddleBoundaryDateMin()
    {
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;

        return middleBoundaryMin.ToString("dd MMM yyyy", _cultureInfo);
    }

    protected string GetMiddleBoundaryDateMax()
    {
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;

        return middleBoundaryMax.ToString("dd MMM yyyy", _cultureInfo);
    }

    protected string GetLowerBoundaryDate()
    {
        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        return lowerBoundary.ToString("dd MMM yyyy", _cultureInfo);
    }

    protected async Task ToggleToMonthlyView(MouseEventArgs _)
    {
        SessionService.FilterPreference.ToggleToMonthlyView();
        SessionService.FilterPreference.BoundaryFilters[TabulationId] = BoundaryFilterMode.Monthly;

        LogSwitch.Debug("Monthly on {0}", TabulationId);

        if (PageRefresh.HasDelegate)
        {
            await PageRefresh.InvokeAsync();
        }
    }

    protected async Task ToggleToWeeklyView(MouseEventArgs _)
    {
        SessionService.FilterPreference.ToggleToWeeklyView();
        SessionService.FilterPreference.BoundaryFilters[TabulationId] = BoundaryFilterMode.Weekly;

        LogSwitch.Debug("Weekly on {0}", TabulationId);

        if (PageRefresh.HasDelegate)
        {
            await PageRefresh.InvokeAsync();
        }
    }

    protected async Task ToggleToDailyView(MouseEventArgs _)
    {
        SessionService.FilterPreference.ToggleToDailyView();
        SessionService.FilterPreference.BoundaryFilters[TabulationId] = BoundaryFilterMode.Daily;

        LogSwitch.Debug("Daily on {0}", TabulationId);

        if (PageRefresh.HasDelegate)
        {
            await PageRefresh.InvokeAsync();
        }
    }

    protected string CurrentWeekIndicator()
    {
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;

        bool isCurrentWeek = DateTimeService.IsWithinCurrentWeek(middleBoundaryMin, middleBoundaryMax);

        return isCurrentWeek
            ? "Minggu Ini"
            : "";
    }

    protected string CurrentMonthIndicator()
    {
        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;

        bool isCurrentMonth = DateTimeService.IsWithinCurrentMonth(upperBoundaryMin, upperBoundaryMax);

        return isCurrentMonth
            ? "Bulan Ini"
            : "";
    }

    protected string TodayIndicator()
    {
        var lowerBoundaryMin = SessionService.FilterPreference.LowerBoundaryDateTime;

        return DateTimeService.IsToday(lowerBoundaryMin)
            ? "Hari Ini"
            : "";
    }
}
