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
    protected string FilterSectionCss => SessionService.FilterPreference.ShowFilters ? "enable" : "filter-section-disable";

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
    public List<RootCauseReportModel> UpperBoundaryCauseReports => FilterCauseReports(_upperBoundaryRootCauseReports);
    public List<RootCauseReportModel> MiddleBoundaryRootCauseReports => FilterCauseReports(_middleBoundaryRootCauseReports);
    public List<RootCauseReportModel> LowerRootCauseReports => FilterCauseReports(_lowerBoundaryRootCauseReports);

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard-wip", PageNavName.Dashboard, PageRoute.Dashboard);
    }

    public void ApplyFilters()
    {
        LogSwitch.Debug("Apply Filters");

        var exclusions = SessionService.FilterPreference.RootCauseExclusion.Exclusion;
        LogSwitch.Debug("Exclusion count {0}", exclusions.Count);

        foreach (var exclusion in exclusions)
        {
            LogSwitch.Debug("Exclude: {0}", exclusion);
        }

        this.StateHasChanged();
    }

    protected override void OnInitialized()
    {
        var currentDate = DateTimeService.DateTimeOffsetNow.DateTime.Date;
        var rootCauses = OptionService.RootCauseOptions;

        SessionService.FilterPreference.SetBoundaryDateTimeDefault(currentDate);
        SessionService.FilterPreference.SetRootCauseExclusion(rootCauses);

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

        GenerateReports(includeUpper, _upperBoundaryApprovalStatusReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryApprovalStatusReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryApprovalStatusReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<ApprovalStatusReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var status in availableStatus)
            {
                var report = ReportService.GenerateApprovalStatusReport(status, boundaryData!);
                reportModels.Add(report);
            }
        }
    }

    private void GenerateRootCauseReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        List<string> availableRootCauses = OptionService.RootCauseOptions.ToList();

        GenerateReports(includeUpper, _upperBoundaryRootCauseReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryRootCauseReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryRootCauseReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<RootCauseReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var rootCause in availableRootCauses)
            {
                var report = ReportService.GenerateRootCauseReport(rootCause, boundaryData);
                reportModels.Add(report);
            }
        }
    }

    private List<RootCauseReportModel> FilterCauseReports(List<RootCauseReportModel> rootCauseReports)
    {
        if (SessionService.FilterPreference.RootCauseExclusion is null)
        {
            return rootCauseReports;
        }

        HashSet<string> exclusions = SessionService.FilterPreference.RootCauseExclusion.Exclusion;

        return rootCauseReports
            .Where(report => !exclusions.Contains(report.RootCause, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}
