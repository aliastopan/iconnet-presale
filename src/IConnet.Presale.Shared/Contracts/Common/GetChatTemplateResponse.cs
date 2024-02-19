#nullable disable

namespace IConnet.Presale.Shared.Contracts.Common;

public record GetChatTemplateResponse(ICollection<ChatTemplateDto> ChatTemplateDtos);

public record ChatTemplateDto
{
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
}
