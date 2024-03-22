using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] IIdentityHttpClient IdentityHttpClient { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly List<UserOperatorModel> _userOperatorModels = [];
    private IQueryable<WorkPaper>? _presaleData;

    protected IQueryable<WorkPaper>? PresaleData => _presaleData;
    protected List<UserOperatorModel> UserOperatorModels => _userOperatorModels;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();

            await GetUserOperators();

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
