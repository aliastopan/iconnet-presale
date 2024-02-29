namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IIdentityHttpClient : IHttpClientBase
{
    Task<HttpResult> SignUpAsync(string username, string password,
        string statusEmployment, string userRole, string jobTitle, string jobShift,
        bool isManagedByAdministrator);
    Task<HttpResult> SignInAsync(string username, string password);
    Task<HttpResult> RefreshAccessAsync(string accessToken, string refreshTokenStr);
}
