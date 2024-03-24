using System.Globalization;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;

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

    private readonly List<ApprovalStatusReportModel> _monthlyApprovalStatusReports = [];
    private readonly List<ApprovalStatusReportModel> _weeklyApprovalStatusReports = [];
    private readonly List<ApprovalStatusReportModel> _dailyApprovalStatusReports = [];

    public List<ApprovalStatusReportModel> MonthlyApprovalStatusReports => _monthlyApprovalStatusReports;
    public List<ApprovalStatusReportModel> WeeklyApprovalStatusReports => _weeklyApprovalStatusReports;
    public List<ApprovalStatusReportModel> DailyApprovalStatusReports => _dailyApprovalStatusReports;

    private List<RootCauseReportModel> _monthlyRootCauseReports = [];
    public List<RootCauseReportModel> MonthlyRootCauseReports => _monthlyRootCauseReports;

    private List<ImportAgingReportModel> _monthlyImportAgingReports = [];
    public List<ImportAgingReportModel> MonthlyImportAgingReports => _monthlyImportAgingReports;

    private List<VerificationAgingReportModel> _monthlyVerificationAgingReports = [];
    public List<VerificationAgingReportModel> MonthlyVerificationAgingReports => _monthlyVerificationAgingReports;

    private List<ChatCallMulaiAgingReportModel> _monthlyChatCallMulaiAgingReports = [];
    public List<ChatCallMulaiAgingReportModel> MonthlyChatCallMulaiAgingReports => _monthlyChatCallMulaiAgingReports;

    private List<ChatCallResponsAgingReportModel> _monthlyChatCallResponsAgingReports = [];
    public List<ChatCallResponsAgingReportModel> MonthlyChatCallResponsAgingReports => _monthlyChatCallResponsAgingReports;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleOperators = UserManager.PresaleOperators;

            _monthlyPresaleData = await DashboardManager.GetPresaleDataFromCurrentMonthAsync();
            _weeklyPresaleData = DashboardManager.GetPresaleDataFromCurrentWeek(_monthlyPresaleData);
            _dailyPresaleData = DashboardManager.GetPresaleDataFromToday(_weeklyPresaleData);

            GenerateStatusApprovalReports();
            GenerateRootCauseReports();
            GenerateImportAgingReports();
            GenerateVerificationAgingReports();
            GenerateChatCallMulaiAgingReports();
            GenerateChatCallResponsAgingReport();

            _isInitialized = true;
        }
    }

    private void GenerateStatusApprovalReports()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var monthlyReport = ReportService.GenerateApprovalStatusReport(status, _monthlyPresaleData!);
            var weeklyReport = ReportService.GenerateApprovalStatusReport(status, _weeklyPresaleData!);
            var dailyReport = ReportService.GenerateApprovalStatusReport(status, _dailyPresaleData!);

            _monthlyApprovalStatusReports.Add(monthlyReport);
            _weeklyApprovalStatusReports.Add(weeklyReport);
            _dailyApprovalStatusReports.Add(dailyReport);
        }
    }

    private void GenerateRootCauseReports()
    {
        List<string> availableRootCauses = OptionService.RootCauseOptions.ToList();

        foreach (var rootCause in availableRootCauses)
        {
            var monthlyReport = ReportService.GenerateRootCauseReport(rootCause, MonthlyPresaleData!);

            _monthlyRootCauseReports.Add(monthlyReport);
        }
    }

    private void GenerateImportAgingReports()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateImportAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyImportAgingReports.Add(agingReport);
            }
        }

        _monthlyImportAgingReports = _monthlyImportAgingReports.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateVerificationAgingReports()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateVerificationAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyVerificationAgingReports.Add(agingReport);
            }
        }

        _monthlyVerificationAgingReports = _monthlyVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateChatCallMulaiAgingReports()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateChatCallMulaiAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyChatCallMulaiAgingReports.Add(agingReport);
            }
        }

        _monthlyChatCallMulaiAgingReports = _monthlyChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    }

    private void GenerateChatCallResponsAgingReport()
    {
        foreach (var user in _presaleOperators)
        {
            var agingReport = ReportService.GenerateChatCallResponsAgingReport(user, MonthlyPresaleData!);

            if (agingReport is not null)
            {
                _monthlyChatCallResponsAgingReports.Add(agingReport);
            }
        }

        _monthlyChatCallResponsAgingReports = _monthlyChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    }
}
