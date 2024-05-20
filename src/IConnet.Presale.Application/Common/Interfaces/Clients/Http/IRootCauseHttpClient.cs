namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IRootCauseHttpClient : IHttpClientBase
{
    Task<HttpResult> GetRootCausesAsync();
    Task<HttpResult> AddRootCauseAsync(int order, string cause, string classification);
    Task<HttpResult> EditRootCauseAsync(Guid rootCauseId, string cause, string classification);
    Task<HttpResult> ToggleOptionsAsync(Guid rootCauseId, bool isDeleted, bool isOnVerification);
}
