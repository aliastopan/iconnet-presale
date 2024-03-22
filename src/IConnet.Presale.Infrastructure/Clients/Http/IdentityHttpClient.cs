using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Contracts;
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
        string statusEmployment, string userRole, string jobTitle,
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
            statusEmployment, userRole, jobTitle,
            isManagedByAdministrator);

        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var requestUri = UriEndpoint.Identity.SignUp;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
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
        var requestUri = UriEndpoint.Identity.SignIn;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

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
        var requestUri = UriEndpoint.Identity.Refresh;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> GetUserOperatorsAsync()
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var requestUri = UriEndpoint.Identity.GetOperators;

        using var responseMessage = await HttpClient.GetAsync(requestUri);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
