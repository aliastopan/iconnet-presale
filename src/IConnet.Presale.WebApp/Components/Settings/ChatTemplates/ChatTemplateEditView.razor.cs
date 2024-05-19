namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditView : ComponentBase
{
    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set;} = default!;

    public Guid ChatTemplateId { get; set; } = default!;
    public ChatTemplateSettingModel ActiveModel = default!;
    public string ContentEditHolder { get; set; } = default!;

    protected void OnContentEditHolderChanged(string content)
    {
        if (ActiveModel is null)
        {
            return;
        }

        ActiveModel.Content = content;
        ActiveModel.ActionSetting = ChatTemplateAction.ChatEdit;

        ContentEditHolder = content;
    }

    protected void EditChat(ChatTemplateSettingModel model)
    {
        ActiveModel = model;
        ContentEditHolder = model.Content;

        LogSwitch.Debug("Editing Model...");
    }

    protected MarkupString GetHtmlString(string chat)
    {
        return (MarkupString) chat.FormatHtmlBreak();
    }
}
