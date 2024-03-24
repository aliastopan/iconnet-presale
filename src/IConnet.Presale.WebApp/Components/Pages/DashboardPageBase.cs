using System.Globalization;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : ComponentBase
{
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

    private List<PresaleOperatorModel> _presaleOperators = [];

    private IQueryable<WorkPaper>? _monthlyPresaleData;
    private IQueryable<WorkPaper>? _weeklyPresaleData;
    private IQueryable<WorkPaper>? _dailyPresaleData;

    public IQueryable<WorkPaper>? MonthlyPresaleData => _monthlyPresaleData;
    public IQueryable<WorkPaper>? WeeklyPresaleData => _weeklyPresaleData;
    public IQueryable<WorkPaper>? DailyPresaleData => _dailyPresaleData;

    private readonly List<ApprovalStatusReportModel> _monthlyApprovalStatusReport = [];
    private readonly List<ApprovalStatusReportModel> _weeklyApprovalStatusReport = [];
    private readonly List<ApprovalStatusReportModel> _dailyApprovalStatusReport = [];

    public List<ApprovalStatusReportModel> MonthlyApprovalStatusReport => _monthlyApprovalStatusReport;
    public List<ApprovalStatusReportModel> WeeklyApprovalStatusReport => _weeklyApprovalStatusReport;
    public List<ApprovalStatusReportModel> DailyApprovalStatusReport => _dailyApprovalStatusReport;

    private List<RootCauseReportModel> _monthlyRootCauseReport = [];
    public List<RootCauseReportModel> MonthlyRootCauseReport => _monthlyRootCauseReport;

    private List<ImportAgingReportModel> _monthlyImportAgingReport = [];
    public List<ImportAgingReportModel> MonthlyImportAgingReport => _monthlyImportAgingReport;

    private List<VerificationAgingReportModel> _monthlyVerificationAgingReport = [];
    public List<VerificationAgingReportModel> MonthlyVerificationAgingReport => _monthlyVerificationAgingReport;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleOperators = UserManager.PresaleOperators;

            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromToday(_weeklyPresaleData);

            GenerateStatusApprovalReport();
            GenerateRootCauseReport();
            GenerateImportAgingReport();
            GenerateVerificationAgingReport();

            _isInitialized = true;
        }
    }

    private void GenerateStatusApprovalReport()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var monthlyReport = ReportService.GenerateApprovalStatusReport(status, _monthlyPresaleData!);
            var weeklyReport = ReportService.GenerateApprovalStatusReport(status, _weeklyPresaleData!);
            var dailyReport = ReportService.GenerateApprovalStatusReport(status, _dailyPresaleData!);

            _monthlyApprovalStatusReport.Add(monthlyReport);
            _weeklyApprovalStatusReport.Add(weeklyReport);
            _dailyApprovalStatusReport.Add(dailyReport);
        }
    }

    private void GenerateRootCauseReport()
    {
        List<string> availableRootCauses = OptionService.RootCauseOptions.ToList();

        foreach (var rootCause in availableRootCauses)
        {
            var monthlyReport = ReportService.GenerateRootCauseReport(rootCause, MonthlyPresaleData!);

            _monthlyRootCauseReport.Add(monthlyReport);
        }
    }

    private void GenerateImportAgingReport()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateImportAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyImportAgingReport.Add(agingReport);
            }
        }

        _monthlyImportAgingReport = _monthlyImportAgingReport.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateVerificationAgingReport()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateAgingVerificationReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyVerificationAgingReport.Add(agingReport);
            }
        }

        _monthlyVerificationAgingReport = _monthlyVerificationAgingReport.OrderByDescending(x => x.Average).ToList();
    }
}
