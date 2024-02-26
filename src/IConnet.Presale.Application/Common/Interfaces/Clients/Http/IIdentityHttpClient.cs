namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IIdentityHttpClient : IHttpClientBase
{
    Task<HttpResult> SignInAsync(string username, string password);
    Task<HttpResult> RefreshAccessAsync(string accessToken, string refreshTokenStr);
}
