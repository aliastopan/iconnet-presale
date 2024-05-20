using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Contracts.Identity;

namespace IConnet.Presale.WebApp.Managers;

public class UserManager
{
    private readonly IIdentityHttpClient _identityHttpClient;
    private List<PresaleOperatorModel> _presaleOperatorModels = [];
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public UserManager(IIdentityHttpClient identityHttpClient)
    {
        _identityHttpClient = identityHttpClient;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public List<PresaleOperatorModel> PresaleOperators => _presaleOperatorModels;

    public async Task SetPresaleOperatorsAsync()
    {
        var httpResult = await _identityHttpClient.GetPresaleOperatorsAsync();

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<GetPresaleOperatorsQueryResponse>(httpResult.Content, _jsonSerializerOptions);
            var presaleOperatorDtos = response!.PresaleOperatorDtos;

            _presaleOperatorModels.Clear();

            foreach (var dto in presaleOperatorDtos)
            {
                _presaleOperatorModels.Add(new PresaleOperatorModel(dto));
            }

            var helpdeskOperators = _presaleOperatorModels.Where(x => x.UserRole == UserRole.Helpdesk);
            var pacOperators = _presaleOperatorModels.Where(x => x.UserRole == UserRole.PAC);

            Log.Information("Setting Presale Operators");
            Log.Information("Presale Operators - Helpdesk: {0}", helpdeskOperators.Count());
            Log.Information("Presale Operators - PAC: {0}", pacOperators.Count());
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
            var extension = problemDetails.GetProblemDetailsExtension();
        }
    }

    public async Task<List<UserAccountModel>> GetUserAccountsAsync()
    {
        List<UserAccountModel> userAccounts = [];

        var httpResult = await _identityHttpClient.GetUserAccountsAsync();

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<GetUserAccountsQueryResponse>(httpResult.Content, _jsonSerializerOptions);
            var userAccountDtos = response!.UserAccountDtos;

            foreach (var dto in userAccountDtos)
            {
                userAccounts.Add(new UserAccountModel(dto));
            }

            Log.Information("User Account(s) {0}", userAccounts.Count);
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
            var extension = problemDetails.GetProblemDetailsExtension();
        }

        return userAccounts;
    }

    public async Task<bool> ChangeUsernameAsync(Guid userAccountId, string newUsername)
    {
        var httpResult = await _identityHttpClient.EditUserAccountAsync(userAccountId, newUsername,
            string.Empty, string.Empty, isChangeUsername: true, false);

        if (httpResult.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
            var extension = problemDetails.GetProblemDetailsExtension();

            foreach (var error in extension.Errors)
            {
                Log.Warning("Error: {0}", error.Message);
            }

            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userAccountId,
        string newPassword, string confirmPassword)
    {
        var httpResult = await _identityHttpClient.EditUserAccountAsync(userAccountId, string.Empty,
            newPassword, confirmPassword, false, isChangePassword: true);

        if (httpResult.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
            var extension = problemDetails.GetProblemDetailsExtension();

            foreach (var error in extension.Errors)
            {
                Log.Warning("Error: {0}", error.Message);
            }

            return false;
        }
    }
}
