namespace IConnet.Presale.Shared.Contracts.Common;

public record EditChatTemplateRequest(Guid ChatTemplateId, string TemplateName, int Sequence, string Content, bool IsDeleted)
    : IEditChatTemplate;