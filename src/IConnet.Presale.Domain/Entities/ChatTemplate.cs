#nullable disable

namespace IConnet.Presale.Domain.Entities;

public class ChatTemplate
{
    public Guid ChatTemplateId { get; set; }
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
}
