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
    private readonly List<UserOperatorModel> _userOperatorModels = [];
    private IQueryable<WorkPaper>? _presaleDataMonthly;
    private List<ApprovalStatusReportModel> _approvalStatusReportModels = [];
    private readonly CultureInfo _culture = new CultureInfo("id-ID");

    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _culture);
    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;

    public List<UserOperatorModel> UserOperatorModels => _userOperatorModels;
    public IQueryable<WorkPaper>? PresaleDataMonthly => _presaleDataMonthly;
    public List<ApprovalStatusReportModel> ApprovalStatusReportModels => _approvalStatusReportModels;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleDataMonthly = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();

            await GetUserOperators();

            if (PresaleDataMonthly is null)
            {
                return;
            }

            var reportInProgress = ReportService.GenerateApprovalStatusReport(ApprovalStatus.InProgress, PresaleDataMonthly);
            var reportClosedLost = ReportService.GenerateApprovalStatusReport(ApprovalStatus.CloseLost, PresaleDataMonthly);
            var reportRejected = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Reject, PresaleDataMonthly);
            var reportExpansion = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Expansion, PresaleDataMonthly);
            var reportApproved = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Approve, PresaleDataMonthly);

            _approvalStatusReportModels.Add(reportInProgress);
            _approvalStatusReportModels.Add(reportClosedLost);
            _approvalStatusReportModels.Add(reportRejected);
            _approvalStatusReportModels.Add(reportExpansion);
            _approvalStatusReportModels.Add(reportApproved);


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
