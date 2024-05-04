namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class RootCauseSetting : ComponentBase
{
    [Inject] AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] OptionService OptionService { get; set; } = default!;
    [Inject] SessionService SessionService { get; set; } = default!;

    [Parameter]
    public IQueryable<RootCauseSettingModel>? Models { get; set; }

    [Parameter]
    public EventCallback OnRootCauseAdded { get; set; }

    public bool IsLoading { get; set; } = false;
    public bool EnableApplyOptions => HasToggledOptions();

    protected string GridTemplateCols => GetGridTemplateCols();

    protected bool HasToggledOptions()
    {
        return Models!.Any(x => x.IsToggledSoftDeletion || x.IsToggledOnVerification);
    }

    protected async Task OpenCreateRootCauseDialogAsync()
    {
        if (Models is null)
        {
            return;
        }

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

        IsLoading = true;

        int highestOrder = Models.Max(x => x.Order) + 1;

        bool isSuccess = await RootCauseManager.AddRootCauseAsync(
            highestOrder,
            dialogData.RootCause,
            dialogData.Classification);

        if (isSuccess)
        {
            await RootCauseManager.SetRootCausesAsync();

            if (OnRootCauseAdded.HasDelegate)
            {
                await OnRootCauseAdded.InvokeAsync();
            }

            var updatedRootCauses = OptionService.RootCauseOptions;

            SessionService.FilterPreference.SetRootCauseExclusion(updatedRootCauses, allowOverwrite: true);
        }

        IsLoading = false;
    }

    protected async Task OpenCreateClassificationDialogAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Add Classification",
            TrapFocus = true,
            Width = "600px",
        };

        var dialog = await DialogService.ShowDialogAsync<CreateClassificationDialog>(null!, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (string)result.Data;

        // LogSwitch.Debug("NEW CLASSIFICATION: {0}", dialogData);
        AppSettingsService.AddRootCauseClassification(dialogData);

        await AppSettingsService.SaveChangesAsync();
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
