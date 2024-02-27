namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IRepresentativeOfficeHttpClient : IHttpClientBase
{
    Task<HttpResult> GetRepresentativeOfficesAsync();
}
