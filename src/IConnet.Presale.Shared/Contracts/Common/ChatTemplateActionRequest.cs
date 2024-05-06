namespace IConnet.Presale.Shared.Contracts.Common;

public record ChatTemplateActionRequest(Guid ChatTemplateId, string TemplateName,
    int Sequence, string Content, int Action)
    : IChatTemplateActionModel;
