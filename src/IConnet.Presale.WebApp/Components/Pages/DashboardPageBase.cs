using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : ComponentBase
{
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly CultureInfo _cultureIndonesia = new CultureInfo("id-ID");

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _cultureIndonesia);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    private IQueryable<WorkPaper>? _monthlyPresaleData;
    private IQueryable<WorkPaper>? _weeklyPresaleData;
    private IQueryable<WorkPaper>? _dailyPresaleData;

    public IQueryable<WorkPaper>? MonthlyPresaleData => _monthlyPresaleData;
    public IQueryable<WorkPaper>? WeeklyPresaleData => _weeklyPresaleData;
    public IQueryable<WorkPaper>? DailyPresaleData => _dailyPresaleData;

    private readonly List<ApprovalStatusReportModel> _monthlyApprovalStatusReport = [];
    private readonly List<ApprovalStatusReportModel> _weeklyApprovalStatusReport = [];
    private readonly List<ApprovalStatusReportModel> _dailyApprovalStatusReport = [];

    public List<ApprovalStatusReportModel> MonthlyApprovalStatusReport => _monthlyApprovalStatusReport;
    public List<ApprovalStatusReportModel> WeeklyApprovalStatusReport => _weeklyApprovalStatusReport;
    public List<ApprovalStatusReportModel> DailyApprovalStatusReport => _dailyApprovalStatusReport;

    private List<RootCauseReportModel> _monthlyRootCauseReport = [];
    public List<RootCauseReportModel> MonthlyRootCauseReport => _monthlyRootCauseReport;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromToday(_weeklyPresaleData);

            GenerateStatusApprovalReport();
            GenerateRootCauseReport();

            _isInitialized = true;
        }
    }

    private void GenerateStatusApprovalReport()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var monthlyReport = ReportService.GenerateApprovalStatusReport(status, _monthlyPresaleData!);
            var weeklyReport = ReportService.GenerateApprovalStatusReport(status, _weeklyPresaleData!);
            var dailyReport = ReportService.GenerateApprovalStatusReport(status, _dailyPresaleData!);

            _monthlyApprovalStatusReport.Add(monthlyReport);
            _weeklyApprovalStatusReport.Add(weeklyReport);
            _dailyApprovalStatusReport.Add(dailyReport);
        }
    }

    private void GenerateRootCauseReport()
    {
        List<string> availableRootCauses = OptionService.RootCauseOptions.ToList();

        foreach (var rootCause in availableRootCauses)
        {
            var monthlyReport = ReportService.GenerateRootCauseReport(rootCause, MonthlyPresaleData!);

            _monthlyRootCauseReport.Add(monthlyReport);
        }
    }
}
