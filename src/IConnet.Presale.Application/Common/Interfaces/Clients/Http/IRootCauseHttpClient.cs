namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IRootCauseHttpClient : IHttpClientBase
{
    Task<HttpResult> GetRootCausesAsync();
}
