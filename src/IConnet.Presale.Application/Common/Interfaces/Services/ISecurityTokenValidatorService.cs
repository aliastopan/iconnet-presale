using Microsoft.IdentityModel.Tokens;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface ISecurityTokenValidatorService
{
    TokenValidationParameters GetAccessTokenValidationParameters();
    TokenValidationParameters GetRefreshTokenValidationParameters();
}
