namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class CreateRootCauseDialog : ComponentBase, IDialogContentComponent<NewRootCauseModel>
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] OptionService OptionService { get; set; } = default!;

    [Parameter]
    public NewRootCauseModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> RootCauseClassifications => AppSettingsService.RootCauseClassifications;
    protected string NewRootCause { get; set; } = default!;
    protected string RootCauseClassification { get; set; } = default!;
    protected string? ErrorMessage { get; set; } = default!;

    protected override void OnInitialized()
    {
        RootCauseClassification = AppSettingsService.RootCauseClassifications.First();
    }

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(new NewRootCauseModel(NewRootCause, RootCauseClassification));
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnNewRootCauseChanged(string rootCause)
    {
        ErrorMessage = null;

        rootCause = rootCause
            .SanitizeOnlyAlphanumericAndSpaces()
            .CapitalizeFirstLetterOfEachWord();

        bool hasDuplicate = OptionService.RootCauseOptions.Any(option => option.Equals(rootCause, StringComparison.OrdinalIgnoreCase));

        if (hasDuplicate)
        {
            ErrorMessage = $"Root Cause '{rootCause}' sudah ada";
            return;
        }

        NewRootCause = rootCause;
    }

    protected void OnRootCauseClassificationChanged(string classification)
    {
        RootCauseClassification = classification;
    }
}
