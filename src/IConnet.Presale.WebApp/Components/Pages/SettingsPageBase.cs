namespace IConnet.Presale.WebApp.Components.Pages;

public class SettingsPageBase : ComponentBase, IPageNavigation
{
    [Inject] public RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] public DirectApprovalManager DirectApprovalManager { get; set; } = default!;
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public IQueryable<RootCauseSettingModel>? RootCauseSettingModels { get; set; }
    public IQueryable<DirectApprovalSettingModel>? DirectApprovalSettingModels { get; set; }

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("settings", PageNavName.Settings, PageRoute.Settings);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);
    }

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
