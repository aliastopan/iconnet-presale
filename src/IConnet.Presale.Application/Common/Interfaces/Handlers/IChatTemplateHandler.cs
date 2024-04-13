using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IChatTemplateHandler
{
    Result<ICollection<ChatTemplate>> TryGetChatTemplates(string templateName);
    Result<ICollection<ChatTemplate>> TryGetAvailableChatTemplates();
}
