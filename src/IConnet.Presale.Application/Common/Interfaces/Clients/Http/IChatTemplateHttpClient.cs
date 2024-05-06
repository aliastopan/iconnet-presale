namespace IConnet.Presale.Application.Common.Interfaces.Clients.Http;

public interface IChatTemplateHttpClient : IHttpClientBase
{
    Task<HttpResult> GetChatTemplatesAsync(string templateName);
    Task<HttpResult> GetAvailableChatTemplatesAsync();
    Task<HttpResult> ChatTemplateActionAsync(Guid chatTemplateId, string templateName,
        int sequence, string content, int action);
}
