namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditViewDialog : IDialogContentComponent<List<ChatTemplateSettingModel>>
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;

    [Parameter]
    public List<ChatTemplateSettingModel> Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

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
        return (MarkupString) chat
            .FormatHtmlBreak()
            .FormatHtmlBold()
            .FormatHtmlItalic();
    }
}
