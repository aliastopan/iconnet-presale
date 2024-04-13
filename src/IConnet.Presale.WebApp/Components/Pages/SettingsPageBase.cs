namespace IConnet.Presale.WebApp.Components.Pages;

public class SettingsPageBase : ComponentBase
{
    [Inject] public RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] public DirectApprovalManager DirectApprovalManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public IQueryable<RootCauseSettingModel>? RootCauseSettingModels { get; set; }
    public IQueryable<DirectApprovalSettingModel>? DirectApprovalSettingModels { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        await ReloadRootCauseAsync();
        await ReloadDirectApprovalAsync();

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
}
