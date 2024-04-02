using System.Globalization;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly CultureInfo _cultureIndonesia = new CultureInfo("id-ID");

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _cultureIndonesia);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    protected PresaleDataBoundaryFilter PresaleDataBoundaryFilter { get; set; } = default!;

    private IQueryable<WorkPaper>? _upperBoundaryPresaleData;
    private IQueryable<WorkPaper>? _middleBoundaryPresaleData;
    private IQueryable<WorkPaper>? _lowerBoundaryPresaleData;

    public IQueryable<WorkPaper>? UpperBoundaryPresaleData => _upperBoundaryPresaleData;
    public IQueryable<WorkPaper>? MiddleBoundaryPresaleData => _middleBoundaryPresaleData;
    public IQueryable<WorkPaper>? LowerBoundaryPresaleData => _lowerBoundaryPresaleData;

    private readonly List<ApprovalStatusReportModel> _upperBoundaryApprovalStatusReports = [];
    private readonly List<ApprovalStatusReportModel> _middleBoundaryApprovalStatusReports = [];
    private readonly List<ApprovalStatusReportModel> _lowerBoundaryApprovalStatusReports = [];

    public List<ApprovalStatusReportModel> UpperBoundaryApprovalStatusReports => _upperBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> MiddleBoundaryApprovalStatusReports => _middleBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> LowerBoundaryApprovalStatusReports => _lowerBoundaryApprovalStatusReports;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _upperBoundaryPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _middleBoundaryPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_upperBoundaryPresaleData);
            _lowerBoundaryPresaleData = DashboardManager.GetPresaleDataFromToday(_upperBoundaryPresaleData);

            GenerateStatusApprovalReports();

            _isInitialized = true;
        }
    }

    private void GenerateStatusApprovalReports()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var monthlyReport = ReportService.GenerateApprovalStatusReport(status, _upperBoundaryPresaleData!);
            var weeklyReport = ReportService.GenerateApprovalStatusReport(status, _middleBoundaryPresaleData!);
            var dailyReport = ReportService.GenerateApprovalStatusReport(status, _lowerBoundaryPresaleData!);

            _upperBoundaryApprovalStatusReports.Add(monthlyReport);
            _middleBoundaryApprovalStatusReports.Add(weeklyReport);
            _lowerBoundaryApprovalStatusReports.Add(dailyReport);
        }
    }
}
