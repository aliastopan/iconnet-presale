using System.Security.Claims;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IAccessTokenService
{
    string GenerateAccessToken(UserAccount user);
    Result TryValidateAccessToken(string accessToken);
    ClaimsPrincipal? GetPrincipalFromToken(string accessToken);
}
