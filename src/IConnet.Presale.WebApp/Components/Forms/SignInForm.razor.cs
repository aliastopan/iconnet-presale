using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class SignInForm
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] public ProtectedLocalStorage LocalStorage { get; set; } = default!;
    [Inject] public IIdentityHttpClientService IdentityHttpClientService { get; set; } = default!;

    [Parameter]
    public EventCallback ToggleGuestMode { get; set; }

    private readonly SignInModel _signInForm = new SignInModel();

    public bool IsLoading { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;

    private async Task SubmitAsync()
    {
        ErrorMessage = string.Empty;

        var isValid = _signInForm.TryValidate(out Error[] errors);
        if (!isValid)
        {
            ErrorMessage = SignInModel.SummarizeErrorMessage(errors);
            return;
        }

        await SignInAsync();
    }

    public async Task<string> SignInAsync()
    {
        IsLoading = true;

        var httpResult = await IdentityHttpClientService.SignInAsync(
            _signInForm.Username,
            _signInForm.Password);

        if (httpResult.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = JsonSerializer.Deserialize<SignInResponse>(httpResult.Content, options);
            var accessToken = response!.AccessToken;
            var refreshToken = response!.RefreshTokenStr;

            await LocalStorage.SetAsync("access-token", accessToken);
            await LocalStorage.SetAsync("refresh-token", refreshToken);

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (authenticationState.User.Identity!.IsAuthenticated)
            {
                Log.Information("Redirecting...");
            }

            IsLoading = false;
            return accessToken!;
        }

        IsLoading = false;
        return httpResult.Content;
    }
}
