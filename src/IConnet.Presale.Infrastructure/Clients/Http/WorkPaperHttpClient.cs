using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Interfaces.Models.Presales;
using IConnet.Presale.Shared.Contracts;
using IConnet.Presale.Shared.Contracts.Presale;
using Mapster;

namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class WorkPaperHttpClient : HttpClientBase, IWorkPaperHttpClient
{
    public WorkPaperHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> InsertWorkPaperAsync(IWorkPaperModel workPaperModel)
    {
        bool isResponding = await IsHostRespondingAsync();

        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = workPaperModel.Adapt<InsertWorkPaperRequest>();
        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var requestUri = UriEndpoint.Presale.InsertWorkPaper;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
