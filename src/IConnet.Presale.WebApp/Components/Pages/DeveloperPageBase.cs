namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] public OptionService OptionService { get; set; } = default!;

    private bool _init = false;

    protected override void OnInitialized()
    {
        if (!_init)
        {
            if (AppSettingsService.RootCauseClassifications.Count > 0)
            {
                foreach (var classification in AppSettingsService.RootCauseClassifications)
                {
                    LogSwitch.Debug(classification);
                }
            }
            else
            {
                // LogSwitch.Debug("ROOT CAUSE CLASSIFICATION IS EMPTY");
            }

            foreach (var option in OptionService.RootCauseOptionStack)
            {
                // LogSwitch.Debug("{0}/{1}", option.rootCause, option.classification);
            }

            _init = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }
}
