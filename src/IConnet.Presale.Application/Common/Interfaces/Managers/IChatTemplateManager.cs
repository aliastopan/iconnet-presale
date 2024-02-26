using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IChatTemplateManager
{
    Result<ICollection<ChatTemplate>> TryGetChatTemplates(string templateName);
}
