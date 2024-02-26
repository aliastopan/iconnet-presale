namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IChatTemplateHttpClient : IHttpClientBase
{
    Task<HttpResult> GetChatTemplatesAsync(string templateName);
}
