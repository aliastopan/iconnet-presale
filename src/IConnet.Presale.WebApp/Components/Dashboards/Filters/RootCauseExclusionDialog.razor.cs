namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class RootCauseExclusionDialog : IDialogContentComponent<RootCauseExclusionModel>
{
    [Parameter]
    public RootCauseExclusionModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> RootCauses => GetFilteredRootCauses();
    protected string RootCauseFilter { get; set; } = string.Empty;
    protected bool ToggleSelection { get; set;}
    protected string ToggleSelectionLabel => ToggleSelection ? "Select All" : "Deselect All";

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnToggleSelection()
    {
        if (ToggleSelection)
        {
            Content.DisableAll();
        }
        else
        {
            Content.EnableAll();
        }

        ToggleSelection = !ToggleSelection;
    }

    protected void OnExclusionChanged(string rootCauses, bool inclusion)
    {
        if (inclusion)
        {
            Content.Inclusion.Add(rootCauses);
        }
        else
        {
            Content.Inclusion.Remove(rootCauses);
        }
    }

    protected List<string> GetFilteredRootCauses()
    {
        List<string> rootCauses = Content.RootCauses
            .Order()
            .ToList();

        if (RootCauseFilter.IsNullOrWhiteSpace())
        {
            return rootCauses;
        }

        return rootCauses.Where(option => option.Contains(RootCauseFilter, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
