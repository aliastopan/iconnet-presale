using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class AccessTokenProvider : IAccessTokenService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ISecurityTokenValidatorService _securityTokenValidatorService;
    private readonly AppSecretSettings _appSecretSettings;

    public AccessTokenProvider(IDateTimeService dateTimeService,
        ISecurityTokenValidatorService securityTokenValidatorService,
        IOptions<AppSecretSettings> appSecretSettings)
    {
        _dateTimeService = dateTimeService;
        _securityTokenValidatorService = securityTokenValidatorService;
        _appSecretSettings = appSecretSettings.Value;

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    }

    public string GenerateAccessToken(UserAccount userAccount)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSecretSettings.MasterKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = new JwtSecurityToken
        (
            issuer: _appSecretSettings.JwtIssuer,
            audience: _appSecretSettings.JwtAudience,
            expires: _dateTimeService.UtcNow.Add(_appSecretSettings.JwtLifeTime),
            claims: CreateClaims(userAccount),
            signingCredentials: signingCredentials
        );

        return jwtHandler.WriteToken(jwt);
    }

    public Result TryValidateAccessToken(string accessToken)
    {
        try
        {
            var validationParameters = _securityTokenValidatorService.GetAccessTokenValidationParameters();
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var securityToken);

            return Result.Ok();
        }
        catch (SecurityTokenException exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Warning);
            return Result.Unauthorized(error);
        }
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(accessToken))
            {
                return null;
            }

            var validationParameters = _securityTokenValidatorService.GetRefreshTokenValidationParameters();
            var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var securityToken);
            if (!HasValidSecurityAlgorithm(securityToken))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool HasValidSecurityAlgorithm(SecurityToken securityToken)
    {
        var securityAlgorithm = SecurityAlgorithms.HmacSha256;
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        return jwtSecurityToken is not null && jwtSecurityToken!.Header.Alg
            .Equals(securityAlgorithm, StringComparison.InvariantCulture);
    }

    private static List<Claim> CreateClaims(UserAccount userAccount)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtClaimTypes.Sub, userAccount.UserAccountId.ToString()),
            new Claim(JwtClaimTypes.UniqueName, userAccount.User.Username),
            new Claim(JwtClaimTypes.Shift, userAccount.User.UserShift.ToString()),
            new Claim(JwtClaimTypes.Role, userAccount.User.UserRole.ToString()),
            new Claim(JwtClaimTypes.JobTitle, userAccount.User.JobTitle)
        };

        foreach (var privilege in userAccount.User.UserPrivileges)
        {
            claims.Add(new Claim(JwtClaimTypes.Privileges, privilege.ToString()));
        }

        return claims;
    }
}
