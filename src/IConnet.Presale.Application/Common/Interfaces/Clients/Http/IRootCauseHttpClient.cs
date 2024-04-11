namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IRootCauseHttpClient : IHttpClientBase
{
    Task<HttpResult> GetRootCausesAsync();
    Task<HttpResult> AddRootCauseAsync(int order, string cause);
    Task<HttpResult> ToggleSoftDeletionAsync(Guid rootCauseId, bool isDeleted);
}
