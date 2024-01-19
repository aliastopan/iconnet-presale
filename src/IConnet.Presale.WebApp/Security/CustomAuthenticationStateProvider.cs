using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.Application.Common.Interfaces.Services;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.WebApp.Services;

namespace IConnet.Presale.WebApp.Security;

public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IdentityClientService _identityClientService;

    public CustomAuthenticationStateProvider(IAccessTokenService accessTokenService,
        ProtectedSessionStorage sessionStorage,
        IdentityClientService identityClientService)
    {
        _accessTokenService = accessTokenService;
        _sessionStorage = sessionStorage;
        _identityClientService = identityClientService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Log.Warning("GetAuthenticationStateAsync");

        var tryGetAccessToken = await TryGetAccessTokenAsync();
        var tryGetRefreshToken = await TryGetRefreshTokenAsync();

        if (tryGetAccessToken.IsFailure() || tryGetRefreshToken.IsFailure())
        {
            Log.Warning("Fail to authenticate.");
            return UnauthenticatedState();
        }

        var accessToken = tryGetAccessToken.Value;
        var refreshToken = tryGetRefreshToken.Value;

        var principal = _accessTokenService.GetPrincipalFromToken(accessToken);
        if (principal is null)
        {
            Log.Warning("Fail to authenticate.");
            return UnauthenticatedState();
        }

        Log.Warning("Authenticating...");
        var tryAuthenticate = _accessTokenService.TryValidateAccessToken(accessToken);
        if (tryAuthenticate.IsFailure())
        {
            Log.Warning("Authentication failed.");
            Log.Warning("{0}", tryAuthenticate.Errors[0].Message);
            Log.Warning("Trying to refresh authentication.");
            var httpResult = await _identityClientService.RefreshAccessAsync(accessToken, refreshToken);
            Log.Warning("Response: {0}", httpResult.IsSuccessStatusCode);
            if (httpResult.IsSuccessStatusCode)
            {
                Log.Warning("Refresh authentication has successful.");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var response = JsonSerializer.Deserialize<RefreshAccessResponse>(httpResult.Content, options);
                accessToken = response!.AccessToken;
                refreshToken = response!.RefreshTokenStr;
                await _sessionStorage.SetAsync("access-token", accessToken);
                await _sessionStorage.SetAsync("refresh-token", refreshToken);
            }
            else
            {
                Log.Warning("Refresh token was invalid.");
                Log.Warning("Fail to authenticate.");
                NotifyAuthenticationStateChanged(Task.FromResult(UnauthenticatedState()));
                return UnauthenticatedState();
            }
        }

        NotifyAuthenticationStateChanged(Task.FromResult(AuthenticatedState(principal)));
        return AuthenticatedState(principal);
    }

    private async Task<Result<string>> TryGetAccessTokenAsync()
    {
        var result = await _sessionStorage.GetAsync<string>("access-token");

        if (result.Success)
        {
            var accessToken = result.Value;
            return Result<string>.Ok(accessToken!);
        }

        return Result<string>.Error();
    }

    private async Task<Result<string>> TryGetRefreshTokenAsync()
    {
        var result = await _sessionStorage.GetAsync<string>("refresh-token");

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
