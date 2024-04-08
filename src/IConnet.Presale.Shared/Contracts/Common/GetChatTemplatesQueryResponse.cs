#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetChatTemplatesQueryResponse(ICollection<ChatTemplateDto> ChatTemplateDtos);

public record ChatTemplateDto
{
    public Guid ChatTemplateId { get; init; }
    public string TemplateName { get; init; }
    public int Sequence { get; init; }
    public string Content { get; init; }
}
