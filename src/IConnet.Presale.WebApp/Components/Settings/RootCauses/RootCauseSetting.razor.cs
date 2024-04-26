using Microsoft.Win32.SafeHandles;

namespace IConnet.Presale.WebApp.Components.Settings.RootCauses;

public partial class RootCauseSetting
{
    [Inject] RootCauseManager RootCauseManager { get; set; } = default!;
    [Inject] OptionService OptionService { get; set; } = default!;
    [Inject] SessionService SessionService { get; set; } = default!;

    [Parameter]
    public IQueryable<RootCauseSettingModel>? Models { get; set; }

    [Parameter]
    public EventCallback OnRootCauseAdded { get; set; }

    public bool IsLoading { get; set; } = false;
    public bool EnableAddRootCause { get; set; }
    public string NewRootCause { get; set; } = string.Empty;
    public bool EnableApplyToggleSoftDeletion => ToggleCheck();

    protected string GridTemplateCols => GetGridTemplateCols();

    protected void OnNewRootCauseChanged(string newRootCause)
    {
        NewRootCause = newRootCause.SanitizeOnlyAlphanumericAndSpaces();
    }

    protected bool ToggleCheck()
    {
        return Models!.Any(x => x.IsToggledSoftDeletion);
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
            if (!model.IsToggledSoftDeletion)
            {
                continue;
            }

            Task<bool> toggleSoftDeletionTask = RootCauseManager.ToggleSoftDeletionAsync(model.RootCauseId, model.SoftDeletionToggleValue);

            tasks.Add(toggleSoftDeletionTask);
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
        return $"{350}px {80}px {80}px;";
    }
}
