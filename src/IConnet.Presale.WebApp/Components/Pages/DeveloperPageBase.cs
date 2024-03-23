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
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly CultureInfo _culture = new CultureInfo("id-ID");
    private readonly List<UserOperatorModel> _userOperatorModels = [];

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _culture);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    private IQueryable<WorkPaper>? _monthlyPresaleData;
    private IQueryable<WorkPaper>? _weeklyPresaleData;
    private IQueryable<WorkPaper>? _dailyPresaleData;

    public IQueryable<WorkPaper>? MonthlyPresaleData => _monthlyPresaleData;
    public IQueryable<WorkPaper>? WeeklyPresaleData => _weeklyPresaleData;
    public IQueryable<WorkPaper>? DailyPresaleData => _dailyPresaleData;

    private List<ApprovalStatusReportModel> _monthlyApprovalStatusReport = [];
    private List<ApprovalStatusReportModel> _weeklyApprovalStatusReport = [];
    private List<ApprovalStatusReportModel> _dailyApprovalStatusReport = [];

    public List<ApprovalStatusReportModel> MonthlyApprovalStatusReport => _monthlyApprovalStatusReport;
    public List<ApprovalStatusReportModel> WeeklyApprovalStatusReport => _weeklyApprovalStatusReport;
    public List<ApprovalStatusReportModel> DailyApprovalStatusReport => _dailyApprovalStatusReport;

    private List<RootCauseReportModel> _monthlyRootCauseReport = [];

    public List<RootCauseReportModel> MonthlyRootCauseReport => _monthlyRootCauseReport;

    public List<UserOperatorModel> UserOperatorModels => _userOperatorModels;


    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_weeklyPresaleData);

            await GetUserOperators();

            var stopwatch = Stopwatch.StartNew();

            GenerateStatusApprovalReports();
            GenerateRootCauseReport();

            stopwatch.Stop();

            LogSwitch.Debug("Execution time {0} ms", stopwatch.ElapsedMilliseconds);

            _isInitialized = true;
        }
    }

    private void GenerateStatusApprovalReports()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var monthlyReport = ReportService.GenerateApprovalStatusReport(status, MonthlyPresaleData!);
            var weeklyReport = ReportService.GenerateApprovalStatusReport(status, WeeklyPresaleData!);
            var dailyReport = ReportService.GenerateApprovalStatusReport(status, DailyPresaleData!);

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

    private async Task GetUserOperators()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await IdentityHttpClient.GetUserOperatorsAsync();

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<GetUserOperatorsQueryResponse>(httpResult.Content, options);
            var userOperatorDtos = response!.UserOperatorDtos;

            foreach (var dto in userOperatorDtos)
            {
                _userOperatorModels.Add(new UserOperatorModel(dto));
            }
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetProblemDetailsExtension();
            LogSwitch.Debug("Error: {0}", extension.Errors.First().Message);
        }
    }
}
