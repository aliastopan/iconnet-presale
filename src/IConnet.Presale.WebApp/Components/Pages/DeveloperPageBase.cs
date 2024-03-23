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

    private IQueryable<WorkPaper>? _presaleDataMonthly;
    private IQueryable<WorkPaper>? _presaleDataWeekly;
    private IQueryable<WorkPaper>? _presaleDataDaily;

    private List<ApprovalStatusReportModel> _approvalStatusReportMonthly = [];
    private List<ApprovalStatusReportModel> _approvalStatusReportWeekly = [];
    private List<ApprovalStatusReportModel> _approvalStatusReportDaily = [];

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _culture);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    public List<UserOperatorModel> UserOperatorModels => _userOperatorModels;
    public IQueryable<WorkPaper>? PresaleDataMonthly => _presaleDataMonthly;
    public IQueryable<WorkPaper>? PresaleDataWeekly => _presaleDataWeekly;
    public IQueryable<WorkPaper>? PresaleDataDaily => _presaleDataDaily;

    public List<ApprovalStatusReportModel> ApprovalStatusReportMonthly => _approvalStatusReportMonthly;
    public List<ApprovalStatusReportModel> ApprovalStatusReportWeekly => _approvalStatusReportWeekly;
    public List<ApprovalStatusReportModel> ApprovalStatusReportDaily => _approvalStatusReportDaily;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleDataMonthly = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _presaleDataWeekly = DashboardManager.GetPresaleDataFromCurrentWeek(_presaleDataMonthly);
            _presaleDataDaily = DashboardManager.GetPresaleDataFromCurrentWeek(_presaleDataWeekly);

            await GetUserOperators();

            List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

            foreach (var status in availableStatus)
            {
                var monthlyReport = ReportService.GenerateApprovalStatusReport(status, PresaleDataMonthly!);
                var weeklyReport = ReportService.GenerateApprovalStatusReport(status, PresaleDataWeekly!);
                var dailyReport = ReportService.GenerateApprovalStatusReport(status, PresaleDataDaily!);

                _approvalStatusReportMonthly.Add(monthlyReport);
                _approvalStatusReportWeekly.Add(weeklyReport);
                _approvalStatusReportDaily.Add(dailyReport);

            }

            _isInitialized = true;
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
