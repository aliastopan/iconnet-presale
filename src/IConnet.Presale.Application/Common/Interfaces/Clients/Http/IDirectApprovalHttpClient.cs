namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IDirectApprovalHttpClient : IHttpClientBase
{
    Task<HttpResult> GetDirectApprovalsAsync();
}
