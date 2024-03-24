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
    [Inject] protected IntervalCalculatorService IntervalCalculatorService { get; set; } = default!;

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

    public List<UserOperatorModel> UserOperators => _userOperatorModels;

    private List<ImportAgingReportModel> _importAgingReport = [];

    public List<ImportAgingReportModel> ImportAgingReport => _importAgingReport;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_weeklyPresaleData);

            await GetUserOperators();

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
