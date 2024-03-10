namespace IConnet.Presale.Infrastructure.Services;

internal class HttpClientBase : IHttpClientBase
{
    private readonly HttpClient _httpClient;

    public HttpClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsHostRespondingAsync()
    {
        try
        {
            using var responseMessage = await _httpClient.GetAsync("/");
            responseMessage.EnsureSuccessStatusCode();

            return true;
        }
        catch (HttpRequestException exception)
        {
            Log.Fatal("Host does not responding: {0}", exception.Message);
            return false;
        }
    }

    public HttpClient HttpClient => _httpClient;
}
