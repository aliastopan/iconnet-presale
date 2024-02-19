namespace IConnet.Presale.Application.Common.Interfaces.Services.Http;

public interface IIdentityHttpClientService : IHttpClientBase
{
    Task<HttpResult> SignInAsync(string username, string password);
    Task<HttpResult> RefreshAccessAsync(string accessToken, string refreshTokenStr);
}
