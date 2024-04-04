using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ApprovalStatusTabulationStack : ComponentBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    [Parameter] public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    [Parameter] public EventCallback OnMiddleBoundaryChanged { get; set; }

    private CultureInfo _cultureInfo = new CultureInfo("id-ID");

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
        LogSwitch.Debug("Monthly");

        IsMonthlySelected = true;
        IsWeeklySelected = false;
        IsDailySelected = false;
    }

    protected void ToggleToWeeklyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Weekly");

        IsMonthlySelected = false;
        IsWeeklySelected = true;
        IsDailySelected = false;
    }

    protected void ToggleToDailyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Daily");

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
}
