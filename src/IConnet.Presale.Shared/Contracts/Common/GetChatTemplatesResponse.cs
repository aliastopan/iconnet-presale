#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetChatTemplatesResponse(ICollection<ChatTemplateDto> ChatTemplateDtos);

public record ChatTemplateDto
{
    public string TemplateName { get; init; }
    public int Sequence { get; init; }
    public string Content { get; init; }
}
