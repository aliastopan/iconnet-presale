namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateSelectView : ComponentBase
{
    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplates { get; set; } = default!;

    protected string TemplateName => ChatTemplates.First().TemplateName;

    protected MarkupString GetHtmlString(string chat)
    {
        return (MarkupString) chat
            .FormatHtmlBreak()
            .FormatHtmlBold()
            .FormatHtmlItalic();
    }
}
