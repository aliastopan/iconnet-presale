using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : ComponentBase, IPageNavigation
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected IPresaleDataBoundaryManager PresaleDataBoundaryManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    private bool _isInitialized = false;

    protected string ActiveTabId { get; set; } = "tab-1";
    protected PresaleDataBoundaryFilter PresaleDataBoundaryFilter { get; set; } = default!;
    protected string UpperBoundarySectionCss => SessionService.FilterPreference.ShowDashboardBoundary ? "enable" : "filter-section-disable";

    private List<PresaleOperatorModel> _presaleOperators = [];

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
    private readonly List<ImportAgingReportModel> _upperBoundaryImportAgingReports = [];
    private readonly List<ImportAgingReportModel> _middleBoundaryImportAgingReports = [];
    private readonly List<ImportAgingReportModel> _lowerBoundaryImportAgingReports = [];
    private readonly List<VerificationAgingReportModel> _upperBoundaryVerificationAgingReports = [];
    private readonly List<VerificationAgingReportModel> _middleBoundaryVerificationAgingReports = [];
    private readonly List<VerificationAgingReportModel> _lowerBoundaryVerificationAgingReports = [];
    private readonly List<ChatCallMulaiAgingReportModel> _upperBoundaryChatCallMulaiAgingReports = [];
    private readonly List<ChatCallMulaiAgingReportModel> _middleBoundaryChatCallMulaiAgingReports = [];
    private readonly List<ChatCallMulaiAgingReportModel> _lowerBoundaryChatCallMulaiAgingReports = [];
    private readonly List<ChatCallResponsAgingReportModel> _upperBoundaryChatCallResponsAgingReports = [];
    private readonly List<ChatCallResponsAgingReportModel> _middleBoundaryChatCallResponsAgingReports = [];
    private readonly List<ChatCallResponsAgingReportModel> _lowerBoundaryChatCallResponsAgingReports = [];
    private readonly List<ApprovalAgingReportModel> _upperBoundaryApprovalAgingReportModels = [];
    private readonly List<ApprovalAgingReportModel> _middleBoundaryApprovalAgingReportModels = [];
    private readonly List<ApprovalAgingReportModel> _lowerBoundaryApprovalAgingReportModels = [];

    public List<ApprovalStatusReportModel> UpperBoundaryApprovalStatusReports => _upperBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> MiddleBoundaryApprovalStatusReports => _middleBoundaryApprovalStatusReports;
    public List<ApprovalStatusReportModel> LowerBoundaryApprovalStatusReports => _lowerBoundaryApprovalStatusReports;
    public List<RootCauseReportModel> UpperBoundaryCauseReports => FilterCauseReports(_upperBoundaryRootCauseReports);
    public List<RootCauseReportModel> MiddleBoundaryRootCauseReports => FilterCauseReports(_middleBoundaryRootCauseReports);
    public List<RootCauseReportModel> LowerRootCauseReports => FilterCauseReports(_lowerBoundaryRootCauseReports);
    public List<ImportAgingReportModel> UpperBoundaryImportAgingReports => _upperBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ImportAgingReportModel> MiddleBoundaryImportAgingReports => _middleBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ImportAgingReportModel> LowerBoundaryImportAgingReports => _lowerBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<VerificationAgingReportModel> UpperBoundaryVerificationAgingReports => _upperBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<VerificationAgingReportModel> MiddleBoundaryVerificationAgingReports => _middleBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<VerificationAgingReportModel> LowerBoundaryVerificationAgingReports => _lowerBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallMulaiAgingReportModel> UpperBoundaryChatCallMulaiAgingReports => _upperBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallMulaiAgingReportModel> MiddleBoundaryChatCallMulaiAgingReports => _middleBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallMulaiAgingReportModel> LowerBoundaryChatCallMulaiAgingReports => _lowerBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallResponsAgingReportModel> UpperBoundaryChatCallResponsAgingReports => _upperBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallResponsAgingReportModel> MiddleBoundaryChatCallResponsAgingReports => _middleBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ChatCallResponsAgingReportModel> LowerBoundaryChatCallResponsAgingReports => _lowerBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public List<ApprovalAgingReportModel> UpperBoundaryApprovalAgingReportModels => _upperBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();
    public List<ApprovalAgingReportModel> MiddleBoundaryApprovalAgingReportModels => _middleBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();
    public List<ApprovalAgingReportModel> LowerBoundaryApprovalAgingReportModels => _lowerBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard-wip", PageNavName.Dashboard, PageRoute.Dashboard);
    }

    public async Task OpenBoundaryFilterDialogAsync()
    {
        await Task.CompletedTask;

        var boundary = SessionService.FilterPreference.BoundaryFilters[ActiveTabId];

        LogSwitch.Debug("Boundary Filtering at {0}", boundary);

        var parameters = new DialogParameters()
        {
            Title = $"{boundary} Filter",
            TrapFocus = true,
            Width = "600px",
        };

        // cache boundary DateTime
        DateTime upperBoundaryMin = new DateTime(SessionService.FilterPreference.UpperBoundaryDateTimeMin.Ticks);
        DateTime upperBoundaryMax = new DateTime(SessionService.FilterPreference.UpperBoundaryDateTimeMax.Ticks);
        DateTime middleBoundaryMin = new DateTime(SessionService.FilterPreference.MiddleBoundaryDateTimeMin.Ticks);
        DateTime middleBoundaryMax = new DateTime(SessionService.FilterPreference.MiddleBoundaryDateTimeMax.Ticks);
        DateTime lowerBoundary = new DateTime(SessionService.FilterPreference.LowerBoundaryDateTime.Ticks);

        var dialog = await DialogService.ShowDialogAsync<BoundaryFilterDialog>(boundary, parameters);
        var result = await dialog.Result;

        // if (result.Data == null)
        // {
        //     LogSwitch.Debug("Dialog result is NULL");
        // }

        if (result.Cancelled)
        {
            SessionService.FilterPreference.UpperBoundaryDateTimeMin = new DateTime(upperBoundaryMin.Ticks);
            SessionService.FilterPreference.UpperBoundaryDateTimeMax = new DateTime(upperBoundaryMax.Ticks);
            SessionService.FilterPreference.MiddleBoundaryDateTimeMin = new DateTime(middleBoundaryMin.Ticks);
            SessionService.FilterPreference.MiddleBoundaryDateTimeMax = new DateTime(middleBoundaryMax.Ticks);
            SessionService.FilterPreference.LowerBoundaryDateTime = new DateTime(lowerBoundary.Ticks);

            LogSwitch.Debug("Cancel Filters");

            return;
        }

        LogSwitch.Debug("Apply Filters");
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

    public void OnActiveTabIdChanged(string tabId)
    {
        ActiveTabId = tabId;
        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);

        LogSwitch.Debug("Changing Tab: {0}", ActiveTabId);
    }

    protected override void OnInitialized()
    {
        var currentDate = DateTimeService.DateTimeOffsetNow.DateTime.Date;
        var rootCauses = OptionService.RootCauseOptions;

        // SessionService.FilterPreference.ToggleToMonthlyView();
        SessionService.FilterPreference.SetBoundaryDateTimeDefault(currentDate);
        SessionService.FilterPreference.SetRootCauseExclusion(rootCauses);

        SessionService.FilterPreference.BoundaryFilters.Clear();
        SessionService.FilterPreference.BoundaryFilters.Add("tab-1", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-2", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-3", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-4", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-5", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-6", BoundaryFilterMode.Monthly);
        SessionService.FilterPreference.BoundaryFilters.Add("tab-7", BoundaryFilterMode.Monthly);

        TabNavigationManager.SelectTab(this);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _presaleOperators = UserManager.PresaleOperators;

            var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
            var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
            var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
            var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
            var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

            _upperBoundaryPresaleData = await PresaleDataBoundaryManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
            _middleBoundaryPresaleData = PresaleDataBoundaryManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
            _lowerBoundaryPresaleData = PresaleDataBoundaryManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

            GenerateStatusApprovalReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateRootCauseReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateImportAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateVerificationAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateChatCallMulaiAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateChatCallResponsAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);
            GenerateApprovalAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);

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

        _upperBoundaryImportAgingReports.Clear();
        _middleBoundaryImportAgingReports.Clear();
        _lowerBoundaryImportAgingReports.Clear();

        _upperBoundaryVerificationAgingReports.Clear();
        _middleBoundaryVerificationAgingReports.Clear();
        _lowerBoundaryVerificationAgingReports.Clear();

        _upperBoundaryChatCallMulaiAgingReports.Clear();
        _middleBoundaryChatCallMulaiAgingReports.Clear();
        _lowerBoundaryChatCallMulaiAgingReports.Clear();

        _upperBoundaryChatCallResponsAgingReports.Clear();
        _middleBoundaryChatCallResponsAgingReports.Clear();
        _lowerBoundaryChatCallResponsAgingReports.Clear();

        _upperBoundaryApprovalAgingReportModels.Clear();
        _middleBoundaryApprovalAgingReportModels.Clear();
        _lowerBoundaryApprovalAgingReportModels.Clear();

        _upperBoundaryPresaleData = await PresaleDataBoundaryManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
        _middleBoundaryPresaleData = PresaleDataBoundaryManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
        _lowerBoundaryPresaleData = PresaleDataBoundaryManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

        GenerateStatusApprovalReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateRootCauseReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateImportAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateVerificationAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateChatCallMulaiAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateChatCallResponsAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateApprovalAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);
    }

    public async Task ReloadMiddleBoundaryAsync()
    {
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;

        _middleBoundaryPresaleData = null;

        _middleBoundaryApprovalStatusReports.Clear();
        _middleBoundaryRootCauseReports.Clear();
        _middleBoundaryImportAgingReports.Clear();
        _middleBoundaryVerificationAgingReports.Clear();
        _middleBoundaryChatCallMulaiAgingReports.Clear();
        _middleBoundaryChatCallResponsAgingReports.Clear();
        _middleBoundaryApprovalAgingReportModels.Clear();

        _middleBoundaryPresaleData = PresaleDataBoundaryManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);

        GenerateStatusApprovalReports(includeMiddle: true);
        GenerateRootCauseReports(includeMiddle: true);
        GenerateImportAgingReports(includeMiddle: true);
        GenerateVerificationAgingReports(includeMiddle: true);
        GenerateChatCallMulaiAgingReports(includeMiddle: true);
        GenerateChatCallResponsAgingReport(includeMiddle: true);
        GenerateApprovalAgingReport(includeMiddle: true);

        // var stash = _upperBoundaryPresaleData?.ToList();
        // _upperBoundaryPresaleData = null!;
        SessionService.FilterPreference.IsBufferLoading = true;

        await Task.Delay(500);
        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);

        // _upperBoundaryPresaleData = stash?.AsQueryable();;
        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
        // LogSwitch.Debug("Finish: {0}", _upperBoundaryPresaleData is null);
    }

    public async Task ReloadLowerBoundaryAsync()
    {
        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        _lowerBoundaryPresaleData = null;

        _lowerBoundaryApprovalStatusReports.Clear();
        _lowerBoundaryRootCauseReports.Clear();
        _lowerBoundaryImportAgingReports.Clear();
        _lowerBoundaryVerificationAgingReports.Clear();
        _lowerBoundaryChatCallMulaiAgingReports.Clear();
        _lowerBoundaryChatCallResponsAgingReports.Clear();
        _lowerBoundaryApprovalAgingReportModels.Clear();

        _lowerBoundaryPresaleData = PresaleDataBoundaryManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

        GenerateStatusApprovalReports(includeLower: true);
        GenerateRootCauseReports(includeLower: true);
        GenerateImportAgingReports(includeLower: true);
        GenerateVerificationAgingReports(includeLower: true);
        GenerateChatCallMulaiAgingReports(includeLower: true);
        GenerateChatCallResponsAgingReport(includeLower: true);
        GenerateApprovalAgingReport(includeLower: true);

        // var stash = _upperBoundaryPresaleData?.ToList();
        // _upperBoundaryPresaleData = null!;
        SessionService.FilterPreference.IsBufferLoading = true;

        await Task.Delay(500);
        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);

        // _upperBoundaryPresaleData = stash?.AsQueryable();;
        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
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

    private void GenerateImportAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        GenerateReports(includeUpper, _upperBoundaryImportAgingReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryImportAgingReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryImportAgingReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<ImportAgingReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var user in _presaleOperators)
            {
                var report = ReportService.GenerateImportAgingReport(user, boundaryData!);

                if (report is not null)
                {
                    reportModels.Add(report);
                }
            }
        }
    }

    private void GenerateVerificationAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        GenerateReports(includeUpper, _upperBoundaryVerificationAgingReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryVerificationAgingReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryVerificationAgingReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<VerificationAgingReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var user in _presaleOperators)
            {
                var report = ReportService.GenerateVerificationAgingReport(user, boundaryData!);

                if (report is not null)
                {
                    reportModels.Add(report);
                }
            }
        }
    }

    private void GenerateChatCallMulaiAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        GenerateReports(includeUpper, _upperBoundaryChatCallMulaiAgingReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryChatCallMulaiAgingReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryChatCallMulaiAgingReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<ChatCallMulaiAgingReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var user in _presaleOperators)
            {
                var report = ReportService.GenerateChatCallMulaiAgingReport(user, boundaryData!);

                if (report is not null)
                {
                    reportModels.Add(report);
                }
            }
        }
    }

    private void GenerateChatCallResponsAgingReport(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        GenerateReports(includeUpper, _upperBoundaryChatCallResponsAgingReports, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryChatCallResponsAgingReports, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryChatCallResponsAgingReports, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<ChatCallResponsAgingReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var user in _presaleOperators)
            {
                var report = ReportService.GenerateChatCallResponsAgingReport(user, boundaryData!);

                if (report is not null)
                {
                    reportModels.Add(report);
                }
            }
        }
    }

    private void GenerateApprovalAgingReport(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
    {
        GenerateReports(includeUpper, _upperBoundaryApprovalAgingReportModels, _upperBoundaryPresaleData!);
        GenerateReports(includeMiddle, _middleBoundaryApprovalAgingReportModels, _middleBoundaryPresaleData!);
        GenerateReports(includeLower, _lowerBoundaryApprovalAgingReportModels, _lowerBoundaryPresaleData!);

        // local function
        void GenerateReports(bool include, List<ApprovalAgingReportModel> reportModels, IQueryable<WorkPaper> boundaryData)
        {
            if (!include)
            {
                return;
            }

            foreach (var user in _presaleOperators)
            {
                var report = ReportService.GenerateApprovalAgingReport(user, boundaryData!);

                if (report is not null)
                {
                    reportModels.Add(report);
                }
            }
        }
    }
}
