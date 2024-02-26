namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class RepresentativeOfficeHttpClient : HttpClientBase, IRepresentativeOfficeHttpClient
{
    public RepresentativeOfficeHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> GetRepresentativeOfficesAsync()
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        using var responseMessage = await HttpClient.GetAsync("/api/representative-offices/get");

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
