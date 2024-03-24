using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Managers;

public class UserManager
{
    private readonly IIdentityHttpClient _identityHttpClient;
    private List<PresaleOperatorModel> _presaleOperatorModels = [];

    public UserManager(IIdentityHttpClient identityHttpClient)
    {
        _identityHttpClient = identityHttpClient;
    }

    public List<PresaleOperatorModel> PresaleOperators => _presaleOperatorModels;

    public async Task SetPresaleOperatorsAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await _identityHttpClient.GetPresaleOperatorsAsync();

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<GetPresaleOperatorsQueryResponse>(httpResult.Content, options);
            var presaleOperatorDtos = response!.PresaleOperatorDtos;

            foreach (var dto in presaleOperatorDtos)
            {
                _presaleOperatorModels.Add(new PresaleOperatorModel(dto));
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
