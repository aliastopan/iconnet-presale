namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateEditViewDialog : ComponentBase, IDialogContentComponent<List<ChatTemplateSettingModel>>
{
    [Inject] IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter]
    public List<ChatTemplateSettingModel> Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected string TemplateName => Content.First().TemplateName;

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

    protected bool IsActiveModel(ChatTemplateSettingModel model)
    {
        if (ActiveModel is null)
        {
            return false;
        }

        return model.ChatTemplateId == ActiveModel.ChatTemplateId;
    }

    protected void OnContentEditHolderChanged(string content)
    {
        if (!content.HasValue())
        {
            content = "SAMPLE TEXT";
        }

        if (ActiveModel is null)
        {
            return;
        }

        ActiveModel.Content = content;

        if (ActiveModel.ActionSetting == ChatTemplateAction.Default)
        {
            ActiveModel.SetAction(ChatTemplateAction.ChatEdit);
        }

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

    public async Task AddChatBubbleAsync()
    {
        await ChatBubbleScrollAsync();
    }

    protected async Task ChatBubbleScrollAsync()
    {
        LogSwitch.Debug("Auto Scrolling...");

        var sequence = Content.Max(x => x.Sequence) + 1;
        var content = "SAMPLE TEXT";

        var newChatBubble = new ChatTemplateSettingModel(
            TemplateName,
            sequence,
            content,
            ChatTemplateAction.ChatAdd);

        Content.Add(newChatBubble);

        var elementId = "chat-bubble-bottom-marker";
        await JsRuntime.InvokeVoidAsync("scrollToElement", elementId);
    }

    protected bool CanBeUndo(ChatTemplateSettingModel settingModel)
    {
        return settingModel.ActionSetting == ChatTemplateAction.ChatAdd;
    }

    protected void UndoAddChatBubble(ChatTemplateSettingModel settingModel)
    {
        Content.Remove(settingModel);
    }

    protected void MarkForDeletion(ChatTemplateSettingModel settingModel)
    {
        settingModel.SetAction(ChatTemplateAction.ChatDelete);
    }

    protected void UndoMarkForDeletion(ChatTemplateSettingModel settingModel)
    {
        settingModel.UndoAction();
    }

    protected bool IsMarkForDeletion(ChatTemplateSettingModel settingModel)
    {
        return settingModel.ActionSetting == ChatTemplateAction.ChatDelete;
    }

    protected string MarkForDeletionStyle(ChatTemplateSettingModel settingModel)
    {
        return settingModel.ActionSetting == ChatTemplateAction.ChatDelete
            ? "background-color: crimson; color: white;"
            : "";
    }
}
