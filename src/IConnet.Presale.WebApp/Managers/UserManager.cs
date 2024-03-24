using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Managers;

public class UserManager
{
    private readonly IIdentityHttpClient _identityHttpClient;
    private List<UserOperatorModel> _userOperatorModels = [];

    public UserManager(IIdentityHttpClient identityHttpClient)
    {
        _identityHttpClient = identityHttpClient;
    }

    public List<UserOperatorModel> UserOperators => _userOperatorModels;

    public async Task SetUserOperatorsAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await _identityHttpClient.GetUserOperatorsAsync();

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
