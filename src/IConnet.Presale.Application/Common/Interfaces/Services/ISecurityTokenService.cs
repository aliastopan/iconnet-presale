using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

[Obsolete("Use 'IAccessTokenService' or 'IRefreshTokenService' instead")]
public interface ISecurityTokenService
{
    string GenerateAccessToken(UserAccount user);
    Result<RefreshToken> TryGenerateRefreshToken(string accessToken, UserAccount user);
    Result<RefreshToken> TryValidateSecurityToken(string accessToken, string refreshTokenStr);
}
