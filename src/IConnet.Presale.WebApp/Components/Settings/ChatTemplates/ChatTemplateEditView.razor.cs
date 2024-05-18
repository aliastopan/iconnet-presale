namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditView : ComponentBase
{
    [Inject] ChatTemplateEditService ChatTemplateEditService { get; set; } = default!;

    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set;} = default!;

    protected MarkupString GetHtmlString(string chat)
    {
        return (MarkupString) chat.FormatHtmlBreak();
    }
}
