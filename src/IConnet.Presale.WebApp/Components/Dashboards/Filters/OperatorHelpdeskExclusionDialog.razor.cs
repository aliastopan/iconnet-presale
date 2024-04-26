namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class OperatorHelpdeskExclusionDialog : IDialogContentComponent<OperatorExclusionModel>
{
   [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public OperatorExclusionModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected List<string> HelpdeskAvailable => GetFilteredUsernames();
    protected string UsernameFilter { get; set; } = string.Empty;
    protected bool ToggleSelection { get; set;}
    protected string ToggleSelectionLabel => ToggleSelection ? "Deselect All" : " Select All";

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

    protected void OnExclusionChanged(string pac, bool inclusion)
    {
        if (inclusion)
        {
            Content.Inclusion.Add(pac);
        }
        else
        {
            Content.Inclusion.Remove(pac);
        }
    }

    protected List<string> GetFilteredUsernames()
    {
        List<string> usernames = Content.Usernames
            .Order()
            .ToList();

        if (UsernameFilter.IsNullOrWhiteSpace())
        {
            return usernames;
        }

        return usernames.Where(option => option.Contains(UsernameFilter, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
