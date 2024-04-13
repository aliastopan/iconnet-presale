using IConnet.Presale.Shared.Contracts;

namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class ChatTemplateHttpClient : HttpClientBase, IChatTemplateHttpClient
{
    public ChatTemplateHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> GetChatTemplatesAsync(string templateName)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        using var responseMessage = await HttpClient.GetAsync($"/api/chat-templates/get-{templateName}");

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> GetAvailableChatTemplatesAsync()
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var requestUri = UriEndpoint.ChatTemplate.GetAvailableChatTemplates;

        using var responseMessage = await HttpClient.GetAsync(requestUri);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
