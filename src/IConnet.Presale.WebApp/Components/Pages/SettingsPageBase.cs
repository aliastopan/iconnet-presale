namespace IConnet.Presale.WebApp.Components.Pages;

public class SettingsPageBase : ComponentBase
{
    [Inject] public RootCauseManager RootCauseManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public IQueryable<RootCauseSettingModel>? RootCauseSettingModels { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        RootCauseSettingModels = await RootCauseManager.GetRootCauseSettingModelsAsync();

        IsInitialized = true;
    }

}
