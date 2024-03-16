using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IWorkPaperHttpClient : IHttpClientBase
{
    Task<HttpResult> InsertWorkPaperAsync(IWorkPaperModel workPaperModel);
}
