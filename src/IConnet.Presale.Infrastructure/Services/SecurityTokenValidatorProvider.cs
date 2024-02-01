using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class SecurityTokenValidatorProvider : ISecurityTokenValidatorService
{
    private readonly AppSecretSettings _appSecretSettings;

    public SecurityTokenValidatorProvider(IOptions<AppSecretSettings> appSecretOptions)
    {
        _appSecretSettings = appSecretOptions.Value;
    }

    public TokenValidationParameters GetAccessTokenValidationParameters()
    {
        var masterKey =  _appSecretSettings.MasterKey;
        return new TokenValidationParameters
        {
            ValidIssuer = _appSecretSettings.JwtIssuer,
            ValidAudience = _appSecretSettings.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(masterKey!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    public TokenValidationParameters GetRefreshTokenValidationParameters()
    {
        var masterKey =  _appSecretSettings.MasterKey;
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(masterKey!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
        };
    }
}
