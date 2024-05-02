namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class CreateRootCauseDialog : IDialogContentComponent<NewRootCauseModel>
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;

    [Parameter]
    public NewRootCauseModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> RootCauseClassifications => AppSettingsService.RootCauseClassifications;
    protected string NewRootCause { get; set; } = default!;
    protected string NewRootCauseClassification { get; set; } = default!;

    protected override void OnInitialized()
    {
        NewRootCauseClassification = AppSettingsService.RootCauseClassifications.First();
    }

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(new NewRootCauseModel(NewRootCause, NewRootCauseClassification));
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnNewRootCauseChanged(string newRootCause)
    {
        NewRootCause = newRootCause.SanitizeOnlyAlphanumericAndSpaces();
    }

    protected void OnNewRootCauseClassificationChanged(string classification)
    {
        NewRootCauseClassification = classification;
    }
}
