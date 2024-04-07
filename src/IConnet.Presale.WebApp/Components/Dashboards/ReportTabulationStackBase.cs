using System.Globalization;
using Microsoft.AspNetCore.Components.Web;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public class ReportTabulationStackBase : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    [Parameter] public EventCallback OnMiddleBoundaryChanged { get; set; }
    [Parameter] public EventCallback OnLowerBoundaryChanged { get; set; }

    private CultureInfo _cultureInfo = new CultureInfo("id-ID");

    protected Appearance MonthlyToggleAppearance => IsMonthlySelected ? Appearance.Accent : Appearance.Neutral;
    protected Appearance WeeklyToggleAppearance => IsWeeklySelected ? Appearance.Accent : Appearance.Neutral;
    protected Appearance DailyToggleAppearance => IsDailySelected ? Appearance.Accent : Appearance.Neutral;

    protected string MonthlyToggleDisplayStyle => IsMonthlySelected ? "" : "display: none";
    protected string WeeklyToggleDisplayStyle => IsWeeklySelected ? "" : "display: none";
    protected string DailyToggleDisplayStyle => IsDailySelected ? "" : "display: none";

    public bool IsMonthlySelected { get; set; } = false;
    public bool IsWeeklySelected { get; set; } = false;
    public bool IsDailySelected { get; set; } = false;

    protected override void OnInitialized()
    {
        IsMonthlySelected = true;
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

    protected void ToggleToMonthlyView(MouseEventArgs _)
    {
        IsMonthlySelected = true;
        IsWeeklySelected = false;
        IsDailySelected = false;
    }

    protected void ToggleToWeeklyView(MouseEventArgs _)
    {
        IsMonthlySelected = false;
        IsWeeklySelected = true;
        IsDailySelected = false;
    }

    protected void ToggleToDailyView(MouseEventArgs _)
    {
        IsMonthlySelected = false;
        IsWeeklySelected = false;
        IsDailySelected = true;
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

        bool isCurrentMonth = DateTimeService.IsWithinCurrentWeek(upperBoundaryMin, upperBoundaryMax);

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
