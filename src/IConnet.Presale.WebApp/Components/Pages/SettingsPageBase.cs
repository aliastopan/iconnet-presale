namespace IConnet.Presale.WebApp.Components.Pages;

public class SettingsPageBase : ComponentBase
{
    [Inject] public RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] public DirectApprovalManager DirectApprovalManager { get; set; } = default!;
    [Inject] public ChatTemplateManager ChatTemplateManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public IQueryable<RootCauseSettingModel>? RootCauseSettingModels { get; set; }
    public IQueryable<DirectApprovalSettingModel>? DirectApprovalSettingModels { get; set; }
    public IQueryable<string>? ChatTemplateAvailable { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        await ReloadRootCauseAsync();
        await ReloadDirectApprovalAsync();
        await ReloadChatTemplatesAsync();

        IsInitialized = true;
    }

    protected async Task ReloadRootCauseAsync()
    {
        RootCauseSettingModels = await RootCauseManager.GetRootCauseSettingModelsAsync();
    }

    protected async Task ReloadDirectApprovalAsync()
    {
        DirectApprovalSettingModels = await DirectApprovalManager.GetDirectApprovalSettingModelsAsync();
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

        ChatTemplateAvailable = chatTemplateNames.AsQueryable();
    }
}
