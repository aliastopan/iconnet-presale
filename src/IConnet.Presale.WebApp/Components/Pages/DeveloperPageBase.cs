using System.Diagnostics;
using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;

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

    private CustomerResponseAgingReport _monthlyCustomerResponseAgingReport = default!;
    public CustomerResponseAgingReport MonthlyCustomerResponseAgingReport => _monthlyCustomerResponseAgingReport;

    private List<ChatCallResponsAgingReportModel> _monthlyChatCallResponsAgingReport = [];
    public List<ChatCallResponsAgingReportModel> MonthlyChatCallResponsAgingReport => _monthlyChatCallResponsAgingReport;

    private List<ApprovalAgingReportModel> _monthlyApprovalAgingReportModels = [];
    public List<ApprovalAgingReportModel> MonthlyApprovalAgingReportModels => _monthlyApprovalAgingReportModels;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleOperators = UserManager.PresaleOperators;

            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_weeklyPresaleData);

            var stopwatch = Stopwatch.StartNew();

            GenerateCustomerResponseAgingReport();
            GenerateChatCallResponsAgingReport();
            GenerateApprovalAgingReport();

            stopwatch.Stop();

            LogSwitch.Debug("Execution time {0} ms", stopwatch.ElapsedMilliseconds);

            _isInitialized = true;
        }
    }

    private void GenerateCustomerResponseAgingReport()
    {
        _monthlyCustomerResponseAgingReport = ReportService.GenerateCustomerResponseAgingReport(MonthlyPresaleData!);
    }

    private void GenerateChatCallResponsAgingReport()
    {
        foreach (var user in PresaleOperators)
        {
            var agingReport = ReportService.GenerateChatCallResponsAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyChatCallResponsAgingReport.Add(agingReport);
            }
        }

        _monthlyChatCallResponsAgingReport = _monthlyChatCallResponsAgingReport.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateApprovalAgingReport()
    {
        foreach (var user in PresaleOperators)
        {
            var agingReport = ReportService.GenerateApprovalAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyApprovalAgingReportModels.Add(agingReport);
            }
        }

        _monthlyApprovalAgingReportModels = _monthlyApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();
    }
}
