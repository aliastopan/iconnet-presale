namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditView : ComponentBase
{
    [Inject] ChatTemplateEditService ChatTemplateEditService { get; set; } = default!;

    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set;} = default!;

    public string ChatTemplateEditContent { get; set; } = "SAMPLING";

    protected void OnChatTemplateEditContentChanged(string content)
    {
        ChatTemplateEditContent = content;
    }

    protected MarkupString GetHtmlString(string chat)
    {
        return (MarkupString) chat.FormatHtmlBreak();
    }
}
