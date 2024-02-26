namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class ChatTemplateHttpClient : HttpClientBase, IChatTemplateHttpClient
{
    public ChatTemplateHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> GetChatTemplateAsync(string templateName)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        using var responseMessage = await HttpClient.GetAsync($"/api/chat-template/get-{templateName}");

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
