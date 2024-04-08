using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.WebApp.Models.Presales.Reports;
using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected IPresaleDataBoundaryManager PresaleDataBoundaryManager { get; set; } = default!;
    [Inject] protected IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    private bool _isInitialized = false;
    private readonly CultureInfo _cultureIndonesia = new CultureInfo("id-ID");

    protected int CurrentYear => DateTimeService.DateTimeOffsetNow.Year;
    protected string CurrentMonth => DateTimeService.DateTimeOffsetNow.ToString("MMMM", _cultureIndonesia);
    protected int CurrentWeek => DateTimeService.GetCurrentWeekOfMonth();

    protected bool IsLoading { get; set; } = false;
    public bool IsMonthlySelected { get; set; } = false;
    public bool IsWeeklySelected { get; set; } = false;
    public bool IsDailySelected { get; set; } = false;

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

    protected override void OnInitialized()
    {
        var today = DateTimeService.DateTimeOffsetNow.DateTime;

        SessionService.FilterPreference.SetBoundaryDateTimeDefault(today);

        IsMonthlySelected = true;
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

            _upperBoundaryPresaleData = await PresaleDataBoundaryManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
            _middleBoundaryPresaleData = PresaleDataBoundaryManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
            _lowerBoundaryPresaleData = PresaleDataBoundaryManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

            GenerateStatusApprovalReports();

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

        // this.StateHasChanged();

        // await LoadPresaleDataAsync();

        await Task.CompletedTask;

        this.StateHasChanged();
    }

    public async Task ReloadPresaleDataAsync()
    {
        LogSwitch.Debug("Loading");
        IsLoading = true;

        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
        var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
        var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
        var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

        LogSwitch.Debug("Upper Boundary Min {0}", upperBoundaryMin.Date);
        LogSwitch.Debug("Upper Boundary Max {0}", upperBoundaryMax.Date);

        _upperBoundaryPresaleData = null;
        _middleBoundaryPresaleData = null;
        _lowerBoundaryPresaleData = null;

        _upperBoundaryApprovalStatusReports.Clear();
        _middleBoundaryApprovalStatusReports.Clear();
        _lowerBoundaryApprovalStatusReports.Clear();

        _upperBoundaryPresaleData = await PresaleDataBoundaryManager.GetUpperBoundaryPresaleDataAsync(upperBoundaryMin, upperBoundaryMax);
        _middleBoundaryPresaleData = PresaleDataBoundaryManager.GetMiddleBoundaryPresaleData(_upperBoundaryPresaleData!, middleBoundaryMin, middleBoundaryMax);
        _lowerBoundaryPresaleData = PresaleDataBoundaryManager.GetLowerBoundaryPresaleData(_upperBoundaryPresaleData!, lowerBoundary);

        LogSwitch.Debug("Upper Boundary Count {0}", _upperBoundaryPresaleData!.Count());

        GenerateStatusApprovalReports();

        IsLoading = false;
        LogSwitch.Debug("Finish");
    }

    private void GenerateStatusApprovalReports()
    {
        List<ApprovalStatus> availableStatus = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        foreach (var status in availableStatus)
        {
            var upperBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _upperBoundaryPresaleData!);
            var middleBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _middleBoundaryPresaleData!);
            var lowerBoundaryReport = ReportService.GenerateApprovalStatusReport(status, _lowerBoundaryPresaleData!);

            _upperBoundaryApprovalStatusReports.Add(upperBoundaryReport);
            _middleBoundaryApprovalStatusReports.Add(middleBoundaryReport);
            _lowerBoundaryApprovalStatusReports.Add(lowerBoundaryReport);
        }
    }

    protected void ToggleToMonthlyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Monthly");

        IsMonthlySelected = true;
        IsWeeklySelected = false;
        IsDailySelected = false;
    }

    protected void ToggleToWeeklyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Weekly");

        IsMonthlySelected = false;
        IsWeeklySelected = true;
        IsDailySelected = false;
    }

    protected void ToggleToDailyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Daily");

        IsMonthlySelected = false;
        IsWeeklySelected = false;
        IsDailySelected = true;
    }
}
