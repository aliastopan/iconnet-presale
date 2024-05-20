namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateSetting : ComponentBase
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] IToastService ToastService { get; set; } = default!;
    [Inject] ChatTemplateManager ChatTemplateManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;
    public bool IsLoading { get; set; } = false;

    public IQueryable<string>? ChatTemplateNameAvailable { get; set; }
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set; } = [];

    protected string SwitchTemplateName { get; set; } = default!;
    protected List<ChatTemplateSettingModel> SwitchableChatTemplatesSettings => FilterSwitchableChatTemplate();
    protected Icon ActiveTemplateIcon = new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");

    protected string TargetTemplateName { get; set; } = default!;
    protected List<ChatTemplateSettingModel> EditableChatTemplatesSettings => FilterEditableChatTemplate();

    protected string GridTemplateCols => GetGridTemplateCols();

    protected override void OnInitialized()
    {
        SwitchTemplateName = AppSettingsService.ChatTemplate;
        TargetTemplateName = AppSettingsService.ChatTemplate;
    }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        await ReloadChatTemplatesAsync();

        IsInitialized = true;
    }

    protected async Task ReloadChatTemplatesAsync()
    {
        List<string> chatTemplateNames;
        List<ChatTemplateSettingModel> chatTemplates;

        chatTemplates = await ChatTemplateManager.GetChatTemplateSettingModelsAsync();
        chatTemplateNames = chatTemplates
            .Select(x => x.TemplateName)
            .Distinct()
            .ToList();

        ChatTemplateNameAvailable = chatTemplateNames.Order().AsQueryable();
        ChatTemplatesSettings = new List<ChatTemplateSettingModel>(chatTemplates).OrderBy(x => x.Sequence).ToList();
    }

    protected List<ChatTemplateSettingModel> FilterEditableChatTemplate()
    {
        if (ChatTemplatesSettings.Count == 0)
        {
            return [];
        }

        return ChatTemplatesSettings.Where(x => x.TemplateName == TargetTemplateName).ToList();
    }

    protected List<ChatTemplateSettingModel> FilterSwitchableChatTemplate()
    {
        if (ChatTemplatesSettings.Count == 0)
        {
            return [];
        }

        return ChatTemplatesSettings.Where(x => x.TemplateName == SwitchTemplateName).ToList();
    }

    protected void OnSwitchTemplateNameChanged(string switchTemplateName)
    {
        SwitchTemplateName = switchTemplateName;

        // LogSwitch.Debug("switch template: {0}", SwitchTemplateName);
    }

    protected async Task SelectTemplateNameAsync(string templateName)
    {
        TargetTemplateName = templateName;

        await OpenCreateClassificationDialogAsync();

        // LogSwitch.Debug("edit template: {0}", TargetTemplateName);
    }

    protected async Task ApplySwitchTemplate()
    {
        await AppSettingsService.SwitchChatTemplateAsync(SwitchTemplateName);
        await ChatTemplateManager.SetChatTemplateAsync(SwitchTemplateName);

        SwitchTemplateToast();
    }

    protected async Task OpenCreateClassificationDialogAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Edit Chat Template",
            TrapFocus = true,
            PreventDismissOnOverlayClick = true,
            Width = "800px",
        };

        var dialog = await DialogService.ShowDialogAsync<ChatTemplateEditViewDialog>(EditableChatTemplatesSettings, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (List<ChatTemplateSettingModel>)result.Data;

        var editedModels = dialogData.Where(x => x.ActionSetting == ChatTemplateAction.ChatEdit).ToList();
        var addedModels = dialogData.Where(x => x.ActionSetting == ChatTemplateAction.ChatAdd).ToList();
        var deletedModels = dialogData.Where(x => x.ActionSetting == ChatTemplateAction.ChatDelete).ToList();

        // LogSwitch.Debug("Edited: {0}. Begin applying edit", editedModels.Count);

        IsLoading = true;
        this.StateHasChanged();

        foreach (var editedModel in editedModels)
        {
            await ChatTemplateManager.ApplyChatTemplateAction(editedModel);
        }

        // LogSwitch.Debug("Added: {0}. Begin adding", addedModels.Count);

        foreach (var addedModel in addedModels)
        {
            await ChatTemplateManager.ApplyChatTemplateAction(addedModel);
        }

        // LogSwitch.Debug("Deleted: {0}. Begin deleting", deletedModels.Count);

        foreach (var deletedModel in deletedModels)
        {
            await ChatTemplateManager.ApplyChatTemplateAction(deletedModel);
        }

        await ReloadChatTemplatesAsync();

        IsLoading = false;
        this.StateHasChanged();
    }

    protected bool IsActive(string templateName)
    {
        return templateName == AppSettingsService.ChatTemplate;
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private string GetGridTemplateCols()
    {
        return $"{150}px {130}px;";
    }

    private void SwitchTemplateToast()
    {
        var intent = ToastIntent.Success;
        var message = $"Chat Template telah berhasil diganti.";
        var timeout = 5000;

        ToastService.ShowToast(intent, message, timeout);
    }
}
