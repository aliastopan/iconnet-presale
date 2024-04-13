namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IDirectApprovalHttpClient : IHttpClientBase
{
    Task<HttpResult> GetDirectApprovalsAsync();
    Task<HttpResult> AddDirectApprovalAsync(int order, string description);
    Task<HttpResult> ToggleSoftDeletionAsync(Guid directApprovalId, bool isDeleted);
}
