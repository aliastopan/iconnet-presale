namespace IConnet.Presale.Application.Common.Interfaces.Services.Http;

public interface IChatTemplateHttpClientService : IHttpClientBase
{
    Task<HttpResult> GetChatTemplateAsync(string templateName);
}
