namespace IConnet.Presale.WebApp.Components.Settings;

public partial class RootCauseSetting
{
    [Inject] RootCauseManager RootCauseManager { get; set; } = default!;

    [Parameter]
    public IQueryable<RootCauseSettingModel>? Models { get; set; }

    [Parameter]
    public EventCallback OnRootCauseAdded { get; set; }

    public bool IsLoading { get; set; } = false;
    public bool EnableAddRootCause { get; set; }
    public string NewRootCause { get; set; } = string.Empty;

    protected string GridTemplateCols => GetGridTemplateCols();

    protected void OnNewRootCauseChanged(string newRootCause)
    {
        NewRootCause = newRootCause.SanitizeOnlyAlphanumericAndSpaces();
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

        bool isSuccess = await RootCauseManager.AddRootCauseAsync(highestOrder, rootCause);

        if (isSuccess)
        {
            await RootCauseManager.SetRootCausesAsync();

            if (OnRootCauseAdded.HasDelegate)
            {
                await OnRootCauseAdded.InvokeAsync();
            }
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
