using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Contracts;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class RootCauseHttpClient : HttpClientBase, IRootCauseHttpClient
{
    public RootCauseHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> GetRootCausesAsync()
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var requestUri = UriEndpoint.RootCauses.GetRootCauses;

        using var responseMessage = await HttpClient.GetAsync(requestUri);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> AddRootCauseAsync(int order, string cause)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new AddRootCauseRequest(order, cause);

        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var requestUri = UriEndpoint.RootCauses.AddRootCause;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
