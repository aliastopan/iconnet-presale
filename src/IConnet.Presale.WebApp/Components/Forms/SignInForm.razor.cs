using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class SignInForm : ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] public ProtectedLocalStorage LocalStorage { get; set; } = default!;
    [Inject] public IIdentityHttpClient IdentityHttpClient { get; set; } = default!;

    [Parameter]
    public EventCallback ToggleGuestMode { get; set; }

    private readonly SignInModel _signInForm = new SignInModel();

    public bool IsLoading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
    public SignInModel SignInModel => _signInForm;

    protected async Task SubmitAsync()
    {
        ErrorMessage = string.Empty;

        var isValid = SignInModel.TryValidate(out Error[] errors);
        if (!isValid)
        {
            ErrorMessage = SummarizeErrorMessage(errors);
            return;
        }

        await SignInAsync();
    }

    public async Task SignInAsync()
    {
        IsLoading = true;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await IdentityHttpClient.SignInAsync(
            SignInModel.Username,
            SignInModel.Password);

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<SignInResponse>(httpResult.Content, options);
            string? accessToken = response!.AccessToken;
            string? refreshToken = response!.RefreshTokenStr;

            await LocalStorage.SetAsync("access-token", accessToken);
            await LocalStorage.SetAsync("refresh-token", refreshToken);

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (authenticationState.User.Identity!.IsAuthenticated)
            {
                Log.Information("Redirecting...");
            }
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetProblemDetailsExtension();
            ErrorMessage = extension.Errors.First().Message;
        }

        IsLoading = false;
    }

    private static string SummarizeErrorMessage(Error[] errors)
    {
        if (errors.Length > 1)
        {
            return "Username and Password tidak boleh kosong.";
        }

        return errors.First().Message;
    }
}
