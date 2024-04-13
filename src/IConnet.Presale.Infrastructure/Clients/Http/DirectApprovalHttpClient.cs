using System.Text;
using System.Text.Json;
using IConnet.Presale.Shared.Contracts;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Infrastructure.Clients.Http;

internal sealed class DirectApprovalHttpClient : HttpClientBase, IDirectApprovalHttpClient
{
    public DirectApprovalHttpClient(HttpClient httpClient)
        : base(httpClient)
    {

    }

    public async Task<HttpResult> GetDirectApprovalsAsync()
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var requestUri = UriEndpoint.DirectApproval.GetDirectApprovals;

        using var responseMessage = await HttpClient.GetAsync(requestUri);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> AddDirectApprovalAsync(int order, string description)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new AddDirectApprovalRequest(order, description);

        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var requestUri = UriEndpoint.DirectApproval.AddDirectApproval;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }

    public async Task<HttpResult> ToggleSoftDeletionAsync(Guid directApprovalId, bool isDeleted)
    {
        var isResponding = await IsHostRespondingAsync();
        if (!isResponding)
        {
            return new HttpResult
            {
                IsSuccessStatusCode = false
            };
        }

        var request = new ToggleDirectApprovalSoftDeletionRequest(directApprovalId, isDeleted);

        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var requestUri = UriEndpoint.DirectApproval.ToggleSoftDeletion;

        using var responseMessage = await HttpClient.PostAsync(requestUri, content);

        return new HttpResult
        {
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
            Headers = responseMessage.Headers,
            Content = await responseMessage.Content.ReadAsStringAsync()
        };
    }
}
