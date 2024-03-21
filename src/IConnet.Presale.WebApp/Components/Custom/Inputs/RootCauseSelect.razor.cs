namespace IConnet.Presale.WebApp.Components.Custom.Inputs;

public partial class RootCauseSelect : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;

    private ICollection<string> _rootCauses = default!;

    [Parameter]
    public string SelectedRootCause { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnSelectedRootCause { get; set; }

    protected string RootCauseFilter { get; set; } = string.Empty;
    protected IEnumerable<string> RootCauses => GetFilteredRootCauses();
    protected int DropdownHeight => Math.Min(RootCauses.Count() * 40, 200);
    protected string DropdownHeightPx => $"{DropdownHeight}px";
    protected bool IsPopoverVisible { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsPopoverVisible = true;
        _rootCauses = new List<string>(OptionService.RootCauseOptions);
    }

    protected async Task OnRootCauseChangedAsync(string selectedRootCause)
    {
        SelectedRootCause = selectedRootCause;
        await OnSelectedRootCause.InvokeAsync(selectedRootCause);

        IsPopoverVisible = false;
    }

    protected void OpenPopover()
    {
        IsPopoverVisible = !IsPopoverVisible;
    }

    protected IEnumerable<string> GetFilteredRootCauses()
    {
        if (RootCauseFilter.IsNullOrWhiteSpace())
        {
            return _rootCauses;
        }

        return _rootCauses.Where(option => option.Contains(RootCauseFilter, StringComparison.OrdinalIgnoreCase));
    }
}
