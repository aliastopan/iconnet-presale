namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk page";

    public bool ShowClaims { get; set; } = true;
    public string ToggleText => ShowClaims ? "Hide" : "Show";

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyImportVerified; // needs only claimed fetch mode

        await base.OnInitializedAsync();
    }
}
