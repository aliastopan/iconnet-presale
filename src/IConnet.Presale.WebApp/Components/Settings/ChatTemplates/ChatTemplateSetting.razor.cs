namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateSetting : ComponentBase
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] ChatTemplateEditService ChatTemplateEditService { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ChatTemplateManager ChatTemplateManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public IQueryable<string>? ChatTemplateNameAvailable { get; set; }
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set; } = [];

    protected string SwitchTemplateName { get; set; } = default!;
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

        ChatTemplateNameAvailable = chatTemplateNames.AsQueryable();
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

    protected void OnSwitchTemplateNameChanged(string switchTemplateName)
    {
        SwitchTemplateName = switchTemplateName;

        LogSwitch.Debug("switch template: {0}", SwitchTemplateName);
    }

    protected async Task SelectTemplateNameAsync(string templateName)
    {
        TargetTemplateName = templateName;
        ChatTemplateEditService.ResetStash();

        await OpenCreateClassificationDialogAsync();

        LogSwitch.Debug("edit template: {0}", TargetTemplateName);
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

        LogSwitch.Debug("Edited: {0}", editedModels.Count);
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
}
