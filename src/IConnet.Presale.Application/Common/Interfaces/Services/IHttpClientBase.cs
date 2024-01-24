namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IHttpClientBase
{
    HttpClient HttpClient { get; }

    Task<bool> IsHostRespondingAsync();
}
