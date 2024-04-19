using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class MetricPageBase : ComponentBase
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

    protected PresaleDataBoundaryFilter PresaleDataBoundaryFilter { get; set; } = default!;

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

    public virtual List<ApprovalStatusReportModel> UpperBoundaryApprovalStatusReports => _upperBoundaryApprovalStatusReports;
    public virtual List<ApprovalStatusReportModel> MiddleBoundaryApprovalStatusReports => _middleBoundaryApprovalStatusReports;
    public virtual List<ApprovalStatusReportModel> LowerBoundaryApprovalStatusReports => _lowerBoundaryApprovalStatusReports;
    public virtual List<RootCauseReportModel> UpperBoundaryCauseReports => FilterCauseReports(_upperBoundaryRootCauseReports);
    public virtual List<RootCauseReportModel> MiddleBoundaryRootCauseReports => FilterCauseReports(_middleBoundaryRootCauseReports);
    public virtual List<RootCauseReportModel> LowerRootCauseReports => FilterCauseReports(_lowerBoundaryRootCauseReports);
    public virtual List<ImportAgingReportModel> UpperBoundaryImportAgingReports => _upperBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ImportAgingReportModel> MiddleBoundaryImportAgingReports => _middleBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ImportAgingReportModel> LowerBoundaryImportAgingReports => _lowerBoundaryImportAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<VerificationAgingReportModel> UpperBoundaryVerificationAgingReports => _upperBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<VerificationAgingReportModel> MiddleBoundaryVerificationAgingReports => _middleBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<VerificationAgingReportModel> LowerBoundaryVerificationAgingReports => _lowerBoundaryVerificationAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallMulaiAgingReportModel> UpperBoundaryChatCallMulaiAgingReports => _upperBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallMulaiAgingReportModel> MiddleBoundaryChatCallMulaiAgingReports => _middleBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallMulaiAgingReportModel> LowerBoundaryChatCallMulaiAgingReports => _lowerBoundaryChatCallMulaiAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallResponsAgingReportModel> UpperBoundaryChatCallResponsAgingReports => _upperBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallResponsAgingReportModel> MiddleBoundaryChatCallResponsAgingReports => _middleBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ChatCallResponsAgingReportModel> LowerBoundaryChatCallResponsAgingReports => _lowerBoundaryChatCallResponsAgingReports.OrderByDescending(x => x.Average).ToList();
    public virtual List<ApprovalAgingReportModel> UpperBoundaryApprovalAgingReportModels => _upperBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();
    public virtual List<ApprovalAgingReportModel> MiddleBoundaryApprovalAgingReportModels => _middleBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();
    public virtual List<ApprovalAgingReportModel> LowerBoundaryApprovalAgingReportModels => _lowerBoundaryApprovalAgingReportModels.OrderByDescending(x => x.Average).ToList();

    protected override void OnInitialized()
    {
        var currentDate = DateTimeService.DateTimeOffsetNow.DateTime.Date;
        var rootCauses = OptionService.RootCauseOptions;

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

    protected async Task ResetUpperBoundary()
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
    }

    protected void ResetMiddleBoundary()
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
    }

    protected void ResetLowerBoundary()
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
    }

    protected void GenerateStatusApprovalReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected void GenerateRootCauseReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected List<RootCauseReportModel> FilterCauseReports(List<RootCauseReportModel> rootCauseReports)
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

    protected void GenerateImportAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected void GenerateVerificationAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected void GenerateChatCallMulaiAgingReports(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected void GenerateChatCallResponsAgingReport(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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

    protected void GenerateApprovalAgingReport(bool includeUpper = false, bool includeMiddle = false, bool includeLower = false)
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