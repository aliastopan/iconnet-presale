using Microsoft.Extensions.Options;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RefreshTokenProvider : IRefreshTokenService
{
    private readonly AppSecretSettings _appSecretSettings;
    private readonly AppDbContextFactory _dbContextFactory;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IDateTimeService _dateTimeService;

    public RefreshTokenProvider(IOptions<AppSecretSettings> appSecretSettings,
        AppDbContextFactory dbContextFactory,
        IAccessTokenService accessTokenService,
        IDateTimeService dateTimeService)
    {
        _appSecretSettings = appSecretSettings.Value;
        _dbContextFactory = dbContextFactory;
        _accessTokenService = accessTokenService;
        _dateTimeService = dateTimeService;
    }

    public Result<RefreshToken> TryGenerateRefreshToken(string accessToken, UserAccount userAccount)
    {
        var principal = _accessTokenService.GetPrincipalFromToken(accessToken);
        if (principal is null)
        {
            var error = new Error("Refresh token has invalid null principal.", ErrorSeverity.Warning);
            return Result<RefreshToken>.Unauthorized(error);
        }

        var jti = principal.Claims.Single(x => x.Type == JwtClaimTypes.Jti).Value;
        var refreshToken = new RefreshToken
        {
            RefreshTokenId = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString(),
            Jti = jti,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            ExpiryDate = _dateTimeService.DateTimeOffsetNow.Add(_appSecretSettings.JwtRefreshLifeTime),
            FkUserAccountId = userAccount.UserAccountId,
            UserAccount = userAccount,
        };

        return Result<RefreshToken>.Ok(refreshToken);
    }

    public Result<RefreshToken> TryValidateSecurityToken(string accessToken, string refreshTokenStr)
    {
        var principal = _accessTokenService.GetPrincipalFromToken(accessToken);
        if (principal is null)
        {
            var error = new Error("Refresh token has invalid null principal.", ErrorSeverity.Warning);
            return Result<RefreshToken>.Unauthorized(error);
        }

        RefreshToken? currentRefreshToken;
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            currentRefreshToken = dbContext.GetRefreshToken(token: refreshTokenStr);
        }

        if (currentRefreshToken is null)
        {
            var error = new Error("Refresh token not found.", ErrorSeverity.Warning);
            return Result<RefreshToken>.NotFound(error);
        }

        var errors = Array.Empty<Error>();
        if (currentRefreshToken.ExpiryDate < _dateTimeService.UtcNow)
        {
            var expiredToken = new Error("Refresh token was expired.", ErrorSeverity.Warning);
            errors = [.. errors, expiredToken];
        }

        if (currentRefreshToken.IsInvalidated)
        {
            var invalidatedToken = new Error("Refresh token was invalidated.", ErrorSeverity.Warning);
            errors = [.. errors, invalidatedToken];
        }

        if (currentRefreshToken.IsUsed)
        {
            var usedToken = new Error("Refresh token was used.", ErrorSeverity.Warning);
            errors = [.. errors, usedToken];
        }

        if (errors.Length > 0)
        {
            return Result<RefreshToken>.Invalid(errors);
        }

        currentRefreshToken.IsUsed = true;
        return Result<RefreshToken>.Ok(currentRefreshToken);
    }
}
