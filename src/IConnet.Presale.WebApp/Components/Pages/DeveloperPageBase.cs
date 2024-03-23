using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] protected IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly List<UserOperatorModel> _userOperatorModels = [];
    private IQueryable<WorkPaper>? _presaleData;
    private List<ApprovalStatusReportModel> _approvalStatusReportModels = [];


    public List<UserOperatorModel> UserOperatorModels => _userOperatorModels;
    public IQueryable<WorkPaper>? PresaleData => _presaleData;
    public List<ApprovalStatusReportModel> ApprovalStatusReportModels => _approvalStatusReportModels;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();

            await GetUserOperators();

            if (PresaleData is null)
            {
                return;
            }

            var reportInProgress = ReportService.GenerateApprovalStatusReport(ApprovalStatus.InProgress, PresaleData);
            var reportClosedLost = ReportService.GenerateApprovalStatusReport(ApprovalStatus.CloseLost, PresaleData);
            var reportRejected = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Reject, PresaleData);
            var reportExpansion = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Expansion, PresaleData);
            var reportApproved = ReportService.GenerateApprovalStatusReport(ApprovalStatus.Approve, PresaleData);

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
