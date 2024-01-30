using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IConnet.Presale.Infrastructure.Security;
using IConnet.Presale.Tests.Common.Factories;

namespace IConnet.Presale.Tests.Unit;

public class SecurityTokenTests : UnitTest
{
    private readonly IAccessTokenService _accessTokenService;
    private string _accessToken;

    public SecurityTokenTests()
    {
        _accessTokenService = base.ServicesProvider.GetRequiredService<IAccessTokenService>();

        var userAccount = IdentityFactory.GetTestUserAccount();
        _accessToken = _accessTokenService.GenerateAccessToken(userAccount);
    }

    [Fact]
    public void ClaimJsonToken_ShouldNotBeNull()
    {
        // arrange
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(_accessToken) as JwtSecurityToken;

        // act
        var claim = jsonToken!.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Sub);

        // assert
        claim.Should().NotBeNull();
    }

    [Fact]
    public void AccessToken_ClaimSub_ShouldBe()
    {
        // arrange
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(_accessToken) as JwtSecurityToken;

        // act
        var sub = jsonToken!.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Sub)!.Value;

        // assert
        sub.Should().Be("5d771905-c325-4f9a-adb8-954e0ae21860");
    }

    [Fact]
    public void InvalidStringOfAccessToken_ShouldReturnNullPrincipal()
    {
        // arrange
        var accessToken = "definitely_not_an_accessToken";

        // act
        ClaimsPrincipal? principal = _accessTokenService.GetPrincipalFromToken(accessToken);

        // assert
        principal.Should().BeNull();
    }
}
