using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IRefreshTokenService
{
    Result<RefreshToken> TryGenerateRefreshToken(string accessToken, UserAccount user);
    Result<RefreshToken> TryValidateSecurityToken(string accessToken, string refreshTokenStr);
    Task<int> DeleteUsedRefreshTokensAsync(int daysBefore = 3);
}
