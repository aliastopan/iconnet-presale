namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class CreateClassificationDialog : IDialogContentComponent<string>
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;

    [Parameter]
    public string Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> RootCauseClassifications => AppSettingsService.RootCauseClassifications;
    protected string NewClassification { get; set; } = default!;
    protected string? ErrorMessage { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(NewClassification);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnNewClassificationChanged(string classification)
    {
        ErrorMessage = null;

        classification = classification
            .SanitizeOnlyAlphanumericAndSpaces()
            .ToUpper();

        bool hasDuplicate = RootCauseClassifications.Any(option => option.Equals(classification, StringComparison.OrdinalIgnoreCase));

       if (hasDuplicate)
        {
            ErrorMessage = $"Opsi '{classification}' sudah ada";
            return;
        }

        NewClassification = classification;
    }
}
