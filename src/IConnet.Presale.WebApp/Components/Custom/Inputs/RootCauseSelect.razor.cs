namespace IConnet.Presale.WebApp.Components.Custom.Inputs;

public partial class RootCauseSelect : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;

    private ICollection<string> _rootCauses = default!;
    private readonly int _maxHeight = 40;
    private readonly int _maxSelectionHeight = 200 + 4; // with offset hack

    [Parameter]
    public string SelectedRootCause { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnSelectedRootCauseChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    protected string RootCauseFilter { get; set; } = string.Empty;
    protected IEnumerable<string> RootCauses => GetFilteredRootCauses();
    protected int DropdownHeight => Math.Min(RootCauses.Count() * _maxHeight, _maxSelectionHeight);
    protected string DropdownHeightPx => $"{DropdownHeight}px";
    protected bool IsPopoverVisible { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsPopoverVisible = true;
        _rootCauses = new List<string>(OptionService.RootCauseOptions);

    }

    // protected override void OnAfterRender(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         SelectedRootCause = _rootCauses.First();
    //         LogSwitch.Debug("First render root cause");
    //     }
    // }

    protected async Task OnRootCauseChangedAsync(string selectedRootCause)
    {
        SelectedRootCause = selectedRootCause;
        await OnSelectedRootCauseChanged.InvokeAsync(selectedRootCause);

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

    protected string GetDisplayRootCause()
    {
        return SelectedRootCause.IsNullOrWhiteSpace()
            ? _rootCauses.First()
            : SelectedRootCause;
    }
}
