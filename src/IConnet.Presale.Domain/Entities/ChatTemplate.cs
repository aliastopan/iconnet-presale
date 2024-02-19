#nullable disable

namespace IConnet.Presale.Domain.Entities;

public class ChatTemplate
{
    public ChatTemplate()
    {

    }

    public ChatTemplate(string templateName, int sequence, string content)
    {
        ChatTemplateId = Guid.NewGuid();
        TemplateName = templateName;
        Sequence = sequence;
        Content = content;
    }

    public Guid ChatTemplateId { get; set; }
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
}
