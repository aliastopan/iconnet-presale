namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class EditRootCauseDialog : ComponentBase, IDialogContentComponent<EditRootCauseModel>
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;

    [Parameter]
    public EditRootCauseModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> RootCauseClassifications => AppSettingsService.RootCauseClassifications;
    protected string CurrentClassification { get; set; } = default!;
    protected string Classification { get; set; } = default!;

    protected override void OnInitialized()
    {
        CurrentClassification = Content.Classification;
        Classification = Content.Classification;
    }

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(new EditRootCauseModel(Content.RootCauseId,
            Content.RootCause,
            Classification));
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnClassificationChanged(string classification)
    {
        Classification = classification;
    }
}
