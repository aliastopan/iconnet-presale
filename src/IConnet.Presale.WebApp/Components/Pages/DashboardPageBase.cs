using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : ComponentBase, IPageNavigation
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IDashboardManager DashboardManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    private bool _isInitialized = false;

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
    private readonly List<RootCauseReportModel> _upperBoundaryRootCauseReports = [];
    private readonly List<RootCauseReportModel> _middleBoundaryRootCauseReports = [];
    private readonly List<RootCauseReportModel> _lowerBoundaryRootCauseReports = [];

    public List<ApprovalStatusReportModel> UpperBoundaryApprovalStatusReports => _upperBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> MiddleBoundaryApprovalStatusReports => _middleBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> LowerBoundaryApprovalStatusReports => _lowerBoundaryApprovalStatusReports;
    public List<RootCauseReportModel> UpperBoundaryCauseReports => _upperBoundaryRootCauseReports;
    public List<RootCauseReportModel> MiddleBoundaryRootCauseReports => _middleBoundaryRootCauseReports;
    public List<RootCauseReportModel> LowerRootCauseReports => _lowerBoundaryRootCauseReports;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard-wip", PageNavName.Dashboard, PageRoute.Dashboard);
    }

    protected override void OnInitialized()
    {
        var currentDate = DateTimeService.DateTimeOffsetNow.DateTime.Date;

        SessionService.FilterPreference.SetBoundaryDateTimeDefault(currentDate);

        TabNavigationManager.SelectTab(this);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
            var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
            var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
            var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
            var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

            _upperBoundaryPresaleData = await DashboardManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
            _middleBoundaryPresaleData = DashboardManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
            _lowerBoundaryPresaleData = DashboardManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

            GenerateStatusApprovalReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateRootCauseReports(includeUpper: true, includeMiddle: true, includeLower: true);

            _isInitialized = true;
        }
    }

    public async Task OnUpperBoundaryChangedAsync()
    {
        LogSwitch.Debug("Checking new upper boundary");
        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        LogSwitch.Debug("Upper Boundary Min {0}", upperBoundaryMin.Date);
        LogSwitch.Debug("Upper Boundary Max {0}", upperBoundaryMax.Date);
        LogSwitch.Debug("Middle Boundary Min {0}", middleBoundaryMin.Date);
        LogSwitch.Debug("Middle Boundary Max {0}", middleBoundaryMax.Date);
        LogSwitch.Debug("Lower Boundary {0}", lowerBoundary.Date);

        await Task.CompletedTask;
    }

    public async Task ReloadUpperBoundaryAsync()
    {
        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        _upperBoundaryPresaleData = null;
        _middleBoundaryPresaleData = null;
        _lowerBoundaryPresaleData = null;

        _upperBoundaryApprovalStatusReports.Clear();
        _middleBoundaryApprovalStatusReports.Clear();
        _lowerBoundaryApprovalStatusReports.Clear();
        _upperBoundaryRootCauseReports.Clear();
        _middleBoundaryRootCauseReports.Clear();
        _lowerBoundaryRootCauseReports.Clear();

        _upperBoundaryPresaleData = await DashboardManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
        _middleBoundaryPresaleData = DashboardManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
        _lowerBoundaryPresaleData = DashboardManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

        GenerateStatusApprovalReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateRootCauseReports(includeUpper: true, includeMiddle: true, includeLower: true);
    }

    public async Task ReloadMiddleBoundaryAsync()
    {
        LogSwitch.Debug("Reloading middle boundary");

        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;

        _middleBoundaryPresaleData = null;

        _middleBoundaryApprovalStatusReports.Clear();
        _middleBoundaryRootCauseReports.Clear();

        _middleBoundaryPresaleData = DashboardManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);

        GenerateStatusApprovalReports(includeMiddle: true);
        GenerateRootCauseReports(includeMiddle: true);

        await Task.CompletedTask;
    }

    public async Task ReloadLowerBoundaryAsync()
    {
        LogSwitch.Debug("Reloading lower boundary");

        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        _lowerBoundaryPresaleData = null;

        _lowerBoundaryApprovalStatusReports.Clear();
        _lowerBoundaryRootCauseReports.Clear();

        _lowerBoundaryPresaleData = DashboardManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

        GenerateStatusApprovalReports(includeLower: true);
        GenerateRootCauseReports(includeLower: true);

        await Task.CompletedTask;
    }

    private void GenerateStatusApprovalReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            if (includeUpper)
            {
                var upperBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _upperBoundaryPresaleData!);

                _upperBoundaryApprovalStatusReports.Add(upperBoundaryReport);
            }

            if (includeMiddle)
            {
                var middleBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _middleBoundaryPresaleData!);

                _middleBoundaryApprovalStatusReports.Add(middleBoundaryReport);
            }

            if (includeLower)
            {
                var lowerBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _lowerBoundaryPresaleData!);

                _lowerBoundaryApprovalStatusReports.Add(lowerBoundaryReport);
            }
        }
    }

    private void GenerateRootCauseReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        List<string> availableRootCauses = OptionService.RootCauseOptions.ToList();

        foreach (var rootCause in availableRootCauses)
        {
            if (includeUpper)
            {
                var upperBoundaryReport = ReportService.GenerateRootCauseReport(rootCause, _upperBoundaryPresaleData!);

                _upperBoundaryRootCauseReports.Add(upperBoundaryReport);
            }

            if (includeMiddle)
            {
                var middleBoundaryReport = ReportService.GenerateRootCauseReport(rootCause, _middleBoundaryPresaleData!);

                _middleBoundaryRootCauseReports.Add(middleBoundaryReport);
            }

            if (includeLower)
            {
                var lowerBoundaryReport = ReportService.GenerateRootCauseReport(rootCause, _lowerBoundaryPresaleData!);

                _lowerBoundaryRootCauseReports.Add(lowerBoundaryReport);
            }
        }
    }
}
