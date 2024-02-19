using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IChatTemplateManager
{
    Task<Result<ICollection<ChatTemplate>>> TryGetChatTemplatesAsync(string templateName);
}
