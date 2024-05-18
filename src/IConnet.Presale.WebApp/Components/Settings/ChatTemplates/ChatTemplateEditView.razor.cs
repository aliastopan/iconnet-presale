namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditView : ComponentBase
{
    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set;} = default!;

}
