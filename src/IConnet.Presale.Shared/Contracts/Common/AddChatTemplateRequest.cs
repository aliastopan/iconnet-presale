namespace IConnet.Presale.Shared.Contracts.Common;

public record AddChatTemplateRequest(Guid ChatTemplateId, string TemplateName, int Sequence, string Content)
    : IAddChatTemplate;
