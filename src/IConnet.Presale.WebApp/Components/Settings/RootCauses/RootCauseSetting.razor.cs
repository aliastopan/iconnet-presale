namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class RootCauseSetting : ComponentBase
{
    [Inject] RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] OptionService OptionService { get; set; } = default!;
    [Inject] SessionService SessionService { get; set; } = default!;

    [Parameter]
    public IQueryable<RootCauseSettingModel>? Models { get; set; }

    [Parameter]
    public EventCallback OnRootCauseAdded { get; set; }

    public bool IsLoading { get; set; } = false;
    public bool EnableAddRootCause { get; set; }
    public string NewRootCause { get; set; } = string.Empty;
    public bool EnableApplyOptions => HasToggledOptions();

    protected string GridTemplateCols => GetGridTemplateCols();

    protected void OnNewRootCauseChanged(string newRootCause)
    {
        NewRootCause = newRootCause.SanitizeOnlyAlphanumericAndSpaces();
    }

    protected bool HasToggledOptions()
    {
        return Models!.Any(x => x.IsToggledSoftDeletion || x.IsToggledOnVerification);
    }

    protected async Task OpenCreateRootCauseDialogAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Add Root Cause",
            TrapFocus = true,
            Width = "600px",
        };

        var dialog = await DialogService.ShowDialogAsync<CreateRootCauseDialog>(null!, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (NewRootCauseModel)result.Data;

        LogSwitch.Debug("New Root Cause {0} ({1})", dialogData.RootCause, dialogData.Classification);

        await Task.CompletedTask;
    }

    protected async Task SubmitNewRootCauseAsync()
    {
        if (NewRootCause.IsNullOrWhiteSpace() || Models is null)
        {
            return;
        }

        IsLoading = true;

        int highestOrder = Models.Max(x => x.Order) + 1;
        string rootCause = NewRootCause.CapitalizeFirstLetterOfEachWord();

        // TODO: add root cause classification
        string classification = string.Empty;

        bool isSuccess = await RootCauseManager.AddRootCauseAsync(highestOrder, rootCause, classification);

        if (isSuccess)
        {
            await RootCauseManager.SetRootCausesAsync();

            if (OnRootCauseAdded.HasDelegate)
            {
                await OnRootCauseAdded.InvokeAsync();
            }

            NewRootCause = string.Empty;

            var rootCauses = OptionService.RootCauseOptions;
            SessionService.FilterPreference.SetRootCauseExclusion(rootCauses, allowOverwrite: true);
        }

        IsLoading = false;
    }

    protected async Task ApplyToggleSoftDeletionAsync()
    {
        if (Models is null)
        {
            return;
        }

        IsLoading = true;

        List<Task<bool>> tasks = [];

        foreach (var model in Models)
        {
            bool IsToggleDelete = model.IsToggledSoftDeletion;
            bool IsToggleInclude = model.IsToggledOnVerification;

            if (IsToggleDelete)
            {
                Task<bool> toggleOptionsTask = RootCauseManager.ToggleOptionsAsync(
                    model.RootCauseId,
                    model.SoftDeletionToggleValue,
                    model.OnVerificationToggleValue);

                tasks.Add(toggleOptionsTask);
            }

            if (IsToggleInclude)
            {
                // bool toggleOnVerification = model.IsDeleted
                //     ? false
                //     : model.OnVerificationToggleValue;

                Task<bool> toggleOptionsTask = RootCauseManager.ToggleOptionsAsync(
                    model.RootCauseId,
                    model.SoftDeletionToggleValue,
                    model.OnVerificationToggleValue);

                tasks.Add(toggleOptionsTask);
            }
        }

        bool[] results = await Task.WhenAll(tasks);

        if (results.Any(result => result))
        {
            await RootCauseManager.SetRootCausesAsync();

            if (OnRootCauseAdded.HasDelegate)
            {
                await OnRootCauseAdded.InvokeAsync();
            }

            var rootCauses = OptionService.RootCauseOptions;
            SessionService.FilterPreference.SetRootCauseExclusion(rootCauses, allowOverwrite: true);
        }

        IsLoading = false;
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private string GetGridTemplateCols()
    {
        return $"{350}px {250}px {85}px {85}px {150}px;";
    }
}
