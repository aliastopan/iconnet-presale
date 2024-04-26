namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;

    private bool _init = false;

    protected override void OnInitialized()
    {
        if (!_init)
        {
            foreach (var option in OptionService.RootCauseOptionStack)
            {
                LogSwitch.Debug("{0}/{1}", option.rootCause, option.classification);
            }
            _init = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }
}
