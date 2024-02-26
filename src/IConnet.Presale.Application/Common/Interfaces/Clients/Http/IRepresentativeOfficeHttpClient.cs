namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IRepresentativeOfficeHttpClient
{
    Task<HttpResult> GetRepresentativeOfficesAsync();
}
