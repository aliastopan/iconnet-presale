using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;
using IConnet.Presale.Shared.Contracts.Identity.Registration;

namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class IdentityHttpClient : HttpClientBase, IIdentityHttpClient
{
    public IdentityHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }


    public async Task<HttpResult> SignUpAsync(string username, string password,
        string statusEmployment, string userRole, string jobTitle, string jobShift,
        bool isManagedByAdministrator)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new SignUpRequest(username, password,
            statusEmployment, userRole, jobTitle, jobShift,
            isManagedByAdministrator);

        throw new NotImplementedException();
    }

    public async Task<HttpResult> SignInAsync(string username, string password)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new SignInRequest(username, password);
        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        using var responseMessage = await HttpClient.PostAsync("api/sign-in", content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> RefreshAccessAsync(string accessToken, string refreshTokenStr)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new RefreshAccessRequest(accessToken, refreshTokenStr);
        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        using var responseMessage = await HttpClient.PostAsync("api/sign-in/refresh", content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
