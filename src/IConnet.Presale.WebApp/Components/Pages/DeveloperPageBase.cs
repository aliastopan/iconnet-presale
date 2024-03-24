using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;
    [Inject] protected IntervalCalculatorService IntervalCalculatorService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly CultureInfo _culture = new CultureInfo("id-ID");
    private List<UserOperatorModel> _userOperators = [];

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _culture);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    private IQueryable<WorkPaper>? _monthlyPresaleData;
    private IQueryable<WorkPaper>? _weeklyPresaleData;
    private IQueryable<WorkPaper>? _dailyPresaleData;

    public IQueryable<WorkPaper>? MonthlyPresaleData => _monthlyPresaleData;
    public IQueryable<WorkPaper>? WeeklyPresaleData => _weeklyPresaleData;
    public IQueryable<WorkPaper>? DailyPresaleData => _dailyPresaleData;

    public List<UserOperatorModel> UserOperators => _userOperators;

    private List<ImportAgingReportModel> _importAgingReport = [];

    public List<ImportAgingReportModel> ImportAgingReport => _importAgingReport;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _userOperators = UserManager.UserOperators;

            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_weeklyPresaleData);

            var stopwatch = Stopwatch.StartNew();

            GenerateImportAgingReport();

            stopwatch.Stop();

            LogSwitch.Debug("Execution time {0} ms", stopwatch.ElapsedMilliseconds);

            _isInitialized = true;
        }
    }

    private void GenerateImportAgingReport()
    {
        foreach (var user in UserOperators)
        {
            var agingReport = ReportService.GenerateImportAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _importAgingReport.Add(agingReport);
            }
        }

        _importAgingReport = _importAgingReport.OrderByDescending(x => x.Average).ToList();
    }
}
