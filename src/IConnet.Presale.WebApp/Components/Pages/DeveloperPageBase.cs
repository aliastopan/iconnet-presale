namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] IDashboardManager DashboardManager { get; set; } = default!;

    private bool _isInitialized = false;
    private IQueryable<WorkPaper>? _presaleData;

    protected IQueryable<WorkPaper>? PresaleData => _presaleData;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();

            _isInitialized = true;
        }
    }
}
