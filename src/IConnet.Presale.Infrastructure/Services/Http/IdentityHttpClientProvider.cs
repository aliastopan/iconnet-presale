using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Contracts.Identity.Authentication;

namespace IConnet.Presale.Infrastructure.Services.Http;

internal sealed class IdentityHttpClientProvider : HttpClientBase, IIdentityHttpClientService
{
    public IdentityHttpClientProvider(HttpClient httpClient)
        : base(httpClient)
    {

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
