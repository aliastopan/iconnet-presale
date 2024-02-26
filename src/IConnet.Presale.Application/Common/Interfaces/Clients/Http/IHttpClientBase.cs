namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IHttpClientBase
{
    HttpClient HttpClient { get; }

    Task<bool> IsHostRespondingAsync();
}
