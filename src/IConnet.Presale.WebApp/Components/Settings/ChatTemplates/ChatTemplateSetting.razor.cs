namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateSetting : ComponentBase
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;

    [Parameter]
    public IQueryable<string>? ModelAvailable { get; set; }

    [Parameter]
    public List<ChatTemplateSettingModel> ChatTemplatesSettings { get; set; } = [];

    protected string TargetTemplateName { get; set; } = default!;
    protected Icon ActiveTemplateIcon = new Icons.Filled.Size20.CheckmarkCircle().WithColor("var(--success-green)");

    protected string GridTemplateCols => GetGridTemplateCols();

    protected override void OnInitialized()
    {
        TargetTemplateName = AppSettingsService.ChatTemplate;
    }

    protected void OnTargetTemplateNameChanged(string targetTemplateName)
    {
        TargetTemplateName = targetTemplateName;

        LogSwitch.Debug("template: {0}", TargetTemplateName);
    }

    protected void SelectTemplateName(string templateName)
    {
        LogSwitch.Debug("template: {0}", templateName);
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
