using IConnet.Presale.Shared.Contracts;

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
}
