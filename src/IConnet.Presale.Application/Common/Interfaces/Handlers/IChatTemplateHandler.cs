using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IChatTemplateHandler
{
    Result<ICollection<ChatTemplate>> TryGetChatTemplates(string templateName);
    Result<ICollection<ChatTemplate>> TryGetAvailableChatTemplates();

    Task<Result> TryChatTemplateAction(Guid chatTemplateId, string templateName,
        int sequence, string content, int action);
    // Task<Result> TryAddChatTemplate(Guid chatTemplateId, string templateName, int sequence, string content);
    // Task<Result> TryEditChatTemplate(Guid chatTemplateId, string templateName, int sequence, string content, bool isDeleted);
}
