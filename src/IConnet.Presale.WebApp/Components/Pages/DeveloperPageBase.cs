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
    private List<PresaleOperatorModel> _presaleOperators = [];

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _culture);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    private IQueryable<WorkPaper>? _monthlyPresaleData;
    private IQueryable<WorkPaper>? _weeklyPresaleData;
    private IQueryable<WorkPaper>? _dailyPresaleData;

    public IQueryable<WorkPaper>? MonthlyPresaleData => _monthlyPresaleData;
    public IQueryable<WorkPaper>? WeeklyPresaleData => _weeklyPresaleData;
    public IQueryable<WorkPaper>? DailyPresaleData => _dailyPresaleData;

    public List<PresaleOperatorModel> PresaleOperators => _presaleOperators;

    private List<ImportAgingReportModel> _monthlyImportAgingReport = [];
    public List<ImportAgingReportModel> MonthlyImportAgingReport => _monthlyImportAgingReport;

    private List<VerificationAgingReportModel> _monthlyVerificationAgingReport = [];
    public List<VerificationAgingReportModel> MonthlyVerificationAgingReport => _monthlyVerificationAgingReport;

    private List<ChatCallMulaiAgingReportModel> _monthlyChatCallMulaiAgingReport = [];
    public List<ChatCallMulaiAgingReportModel> MonthlyChatCallMulaiAgingReport => _monthlyChatCallMulaiAgingReport;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleOperators = UserManager.PresaleOperators;

            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_weeklyPresaleData);

            var stopwatch = Stopwatch.StartNew();

            GenerateImportAgingReport();
            GenerateVerificationAgingReport();
            GenerateChatCallMulaiAgingReport();

            stopwatch.Stop();

            LogSwitch.Debug("Execution time {0} ms", stopwatch.ElapsedMilliseconds);

            _isInitialized = true;
        }
    }

    private void GenerateImportAgingReport()
    {
        foreach (var user in PresaleOperators)
        {
            var agingReport = ReportService.GenerateImportAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyImportAgingReport.Add(agingReport);
            }
        }

        _monthlyImportAgingReport = _monthlyImportAgingReport.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateVerificationAgingReport()
    {
        foreach (var user in PresaleOperators)
        {
            var agingReport = ReportService.GenerateVerificationAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyVerificationAgingReport.Add(agingReport);
            }
        }

        _monthlyVerificationAgingReport = _monthlyVerificationAgingReport.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateChatCallMulaiAgingReport()
    {
        foreach (var user in PresaleOperators)
        {
            var agingReport = ReportService.GenerateChatCallMulaiAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyChatCallMulaiAgingReport.Add(agingReport);
            }
        }

        _monthlyChatCallMulaiAgingReport = _monthlyChatCallMulaiAgingReport.OrderByDescending(x => x.Average).ToList();
    }
}
