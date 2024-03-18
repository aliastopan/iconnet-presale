using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;

namespace IConnet.Presale.WebApp.Security;

public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly SessionService _sessionService;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IIdentityHttpClient _identityHttpClient;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage,
        SessionService sessionService,
        IAccessTokenService accessTokenService,
        IIdentityHttpClient identityHttpClient)
    {
        _localStorage = localStorage;
        _sessionService = sessionService;
        _accessTokenService = accessTokenService;
        _identityHttpClient = identityHttpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // LogSwitch.Debug("Authenticating...");

        var tryGetAccessToken = await TryGetAccessTokenAsync();
        var tryGetRefreshToken = await TryGetRefreshTokenAsync();

        if (tryGetAccessToken.IsFailure() || tryGetRefreshToken.IsFailure())
        {
            // LogSwitch.Debug("Fail to authenticate.");
            return UnauthenticatedState();
        }

        string? accessToken = tryGetAccessToken.Value;
        string? refreshToken = tryGetRefreshToken.Value;

        var principal = _accessTokenService.GetPrincipalFromToken(accessToken);
        if (principal is null)
        {
            // LogSwitch.Debug("Authentication has failed.");
            return UnauthenticatedState();
        }

        var tryAuthenticate = _accessTokenService.TryValidateAccessToken(accessToken);
        if (tryAuthenticate.IsFailure())
        {
            // LogSwitch.Debug("Authentication has failed.");
            // LogSwitch.Debug("{0}", tryAuthenticate.Errors[0].Message);
            // LogSwitch.Debug("Trying to refresh authentication.");
            var httpResult = await _identityHttpClient.RefreshAccessAsync(accessToken, refreshToken);
            if (httpResult.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var response = JsonSerializer.Deserialize<RefreshAccessResponse>(httpResult.Content, options);
                accessToken = response!.AccessToken;
                refreshToken = response!.RefreshTokenStr;
                await _localStorage.SetAsync("access-token", accessToken);
                await _localStorage.SetAsync("refresh-token", refreshToken);

                // LogSwitch.Debug("Authentication has successful.");

            }
            else
            {
                await _localStorage.DeleteAsync("access-token");
                await _localStorage.DeleteAsync("refresh-token");

                // LogSwitch.Debug("Authentication has failed (invalid refresh token).");
                NotifyAuthenticationStateChanged(Task.FromResult(UnauthenticatedState()));
                return UnauthenticatedState();
            }
        }

        await _sessionService.SetSessionAsync(principal);

        NotifyAuthenticationStateChanged(Task.FromResult(AuthenticatedState(principal)));
        return AuthenticatedState(principal);
    }

    private async Task<Result<string>> TryGetAccessTokenAsync()
    {
        var result = await _localStorage.GetAsync<string>("access-token");

        if (result.Success)
        {
            var accessToken = result.Value;
            return Result<string>.Ok(accessToken!);
        }

        return Result<string>.Error();
    }

    private async Task<Result<string>> TryGetRefreshTokenAsync()
    {
        var result = await _localStorage.GetAsync<string>("refresh-token");

        if (result.Success)
        {
            var accessToken = result.Value;
            return Result<string>.Ok(accessToken!);
        }

        return Result<string>.Error();
    }

    private static AuthenticationState AuthenticatedState(ClaimsPrincipal principal)
    {
        return new AuthenticationState(principal);
    }

    private static AuthenticationState UnauthenticatedState()
    {
        return new AuthenticationState(new ClaimsPrincipal());
    }
}
