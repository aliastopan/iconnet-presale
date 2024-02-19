#nullable disable

namespace IConnet.Presale.Shared.Contracts.ChatTemplate;

public record GetChatTemplateResponse(ICollection<ChatTemplateDto> ChatTemplates);

public record ChatTemplateDto
{
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
}
