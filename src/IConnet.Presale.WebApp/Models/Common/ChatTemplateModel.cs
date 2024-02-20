namespace IConnet.Presale.WebApp.Models.Common;

public class ChatTemplateModel
{
    public int Sequence { get; set; }
    public string Content { get; set; } = string.Empty;
    public MarkupString HtmlContent { get; set; } = (MarkupString)string.Empty;
}
