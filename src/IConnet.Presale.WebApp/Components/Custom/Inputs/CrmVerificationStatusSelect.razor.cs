namespace IConnet.Presale.WebApp.Components.Custom.Inputs;

public partial class CrmVerificationStatusSelect : ComponentBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;

    private ICollection<string> _verificationStatus = default!;
    private readonly int _maxHeight = 40;
    private readonly int _maxSelectionHeight = 100 + 6; // with offset hack

    [Parameter]
    public string VerificationStatus { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnSelectedVerificationStatusChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    protected Func<string, bool> OptionDisable => option => option == OptionSelect.StatusVerifikasi.MenungguVerifikasi
        && VerificationStatus != OptionSelect.StatusVerifikasi.MenungguVerifikasi;

    protected string VerificationStatusFilter { get; set; } = string.Empty;
    protected IEnumerable<string> VerificationStatusOptions => GetFilteredVerificationStatus();
    protected int DropdownHeight => Math.Min(VerificationStatusOptions.Count() * _maxHeight, _maxSelectionHeight);
    protected string DropdownHeightPx => $"{DropdownHeight}px";
    protected bool IsPopoverVisible { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsPopoverVisible = false;
        _verificationStatus = new List<string>(OptionSelect.StatusVerifikasi.StatusVerifikasiOptions);
    }

    protected async Task OnVerificationStatusChangedAsync(string selectedRootCause)
    {
        VerificationStatus = selectedRootCause;
        await OnSelectedVerificationStatusChanged.InvokeAsync(selectedRootCause);

        IsPopoverVisible = false;
    }

    protected void OpenPopover()
    {
        IsPopoverVisible = !IsPopoverVisible;
    }

    protected IEnumerable<string> GetFilteredVerificationStatus()
    {
        IEnumerable<string> topLevelOptions = OptionSelect.StatusVerifikasi.StatusVerifikasiOptions;
        IEnumerable<string> rootCauseOptions = OptionService.RootCauseOnVerificationOptions;

        _verificationStatus = topLevelOptions.Concat(rootCauseOptions).ToList();

        if (VerificationStatusFilter.IsNullOrWhiteSpace())
        {
            return _verificationStatus;
        }

        return _verificationStatus.Where(option => option.Contains(VerificationStatusFilter, StringComparison.OrdinalIgnoreCase));
    }

    protected string GetDisplayVerificationStatus()
    {
        return VerificationStatus.IsNullOrWhiteSpace()
            ? _verificationStatus.First()
            : VerificationStatus;
    }
}
