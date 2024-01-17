namespace IConnet.Presale.Shared.Contracts.Identity.Authentication;

public record SignInResponse(string AccessToken, string RefreshTokenStr);
