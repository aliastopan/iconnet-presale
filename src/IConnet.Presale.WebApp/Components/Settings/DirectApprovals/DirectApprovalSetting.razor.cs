namespace IConnet.Presale.WebApp.Components.Settings.DirectApprovals;

public partial class DirectApprovalSetting
{
    [Inject] DirectApprovalManager DirectApprovalManager { get; set; } = default!;

    [Parameter]
    public IQueryable<DirectApprovalSettingModel>? Models { get; set; }

    [Parameter]
    public EventCallback OnDirectApprovalAdded { get; set; }

    public bool IsLoading { get; set; } = false;
    public bool EnableAddDirectApproval { get; set; }
    public string NewDirectApproval { get; set; } = string.Empty;
    public bool EnableApplyToggleSoftDeletion => ToggleCheck();

    protected string GridTemplateCols => GetGridTemplateCols();

    protected void OnNewDirectApprovalChanged(string newDirectApproval)
    {
        NewDirectApproval = newDirectApproval.SanitizeOnlyAlphanumericAndSpaces();
    }

    protected bool ToggleCheck()
    {
        return Models!.Any(x => x.IsToggledSoftDeletion);
    }

    protected async Task SubmitNewDirectApprovalAsync()
    {
        if (NewDirectApproval.IsNullOrWhiteSpace() || Models is null)
        {
            return;
        }

        IsLoading = true;

        int highestOrder = Models.Max(x => x.Order) + 1;
        string directApproval = NewDirectApproval.ToUpper();

        bool isSuccess = await DirectApprovalManager.AddDirectApprovalAsync(highestOrder, directApproval);

        if (isSuccess)
        {
            await DirectApprovalManager.SetDirectApprovalsAsync();

            if (OnDirectApprovalAdded.HasDelegate)
            {
                await OnDirectApprovalAdded.InvokeAsync();
            }

            NewDirectApproval = string.Empty;
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

            Task<bool> toggleSoftDeletionTask = DirectApprovalManager.ToggleSoftDeletionAsync(model.DirectApprovalId, model.SoftDeletionToggleValue);

            tasks.Add(toggleSoftDeletionTask);
        }

        bool[] results = await Task.WhenAll(tasks);

        if (results.Any(result => result))
        {
            await DirectApprovalManager.SetDirectApprovalsAsync();

            if (OnDirectApprovalAdded.HasDelegate)
            {
                await OnDirectApprovalAdded.InvokeAsync();
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
