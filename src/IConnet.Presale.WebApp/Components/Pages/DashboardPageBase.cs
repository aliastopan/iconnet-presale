using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : MetricPageBase, IPageNavigation
{
    [Inject] WorksheetService WorksheetService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    protected string ActiveTabId { get; set; } = "tab-1";
    protected bool IsBufferLoading => SessionService.FilterPreference.IsBufferLoading;
    protected bool IsExportLoading { get; set; } = false;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard", PageNavName.Index, PageRoute.Index);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        TabNavigationManager.SelectTab(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public async Task ExportXlsxAsync()
    {
        if (IsExportLoading)
        {
            return;
        }

        IsExportLoading = true;

        var boundary = SessionService.FilterPreference.BoundaryFilters[ActiveTabId];
        var presaleData = GetBoundaryPresaleData(boundary);
        var username = SessionService.GetSessionAlias().ReplaceSpacesWithUnderscores();
        var dateLabel = DateTimeService.GetStringDateToday();

        if (presaleData is null)
        {
            LogSwitch.Debug("No data were found. Export cancelled.");
            return;
        }

        LogSwitch.Debug("Exporting {0} with {1} boundary", ActiveTabId, boundary);

        if (ActiveTabId == "tab-1") // approval status
        {
            var exportTarget = FilterXlsxStatusApprovals(presaleData);
            var xlsxBytes = WorksheetService.GenerateStandardXlsxBytes(exportTarget, "Approval Status");
            var base64 = Convert.ToBase64String(xlsxBytes);
            var fileName = $"Dashboard_StatusApproval_{username}_{dateLabel}.xlsx";

            await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
        }

        if (ActiveTabId == "tab-2") // root cause
        {
            var exportTarget = FilterXlsxRootCauses(presaleData);
            var xlsxBytes = WorksheetService.GenerateStandardXlsxBytes(exportTarget, "Root Cause");
            var base64 = Convert.ToBase64String(xlsxBytes);
            var fileName = $"Dashboard_RootCause_{username}_{dateLabel}.xlsx";

            await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
        }

        if (ActiveTabId == "tab-3") // aging import
        {
            var exportTarget = FilterXlsxAgingImport(presaleData);
            var xlsxBytes = WorksheetService.GenerateAgingImportXlsxBytes(exportTarget);
            var base64 = Convert.ToBase64String(xlsxBytes);
            var fileName = $"Dashboard_AgingImport_{username}_{dateLabel}.xlsx";

            await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
        }

        LogSwitch.Debug("Export success.");
        IsExportLoading = false;
    }

    protected IQueryable<WorkPaper> FilterXlsxStatusApprovals(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.ApprovalStatusExclusion is null)
        {
            return presaleData;
        }

        HashSet<ApprovalStatus> exclusions = SessionService.FilterPreference.ApprovalStatusExclusion.Exclusion;

        return presaleData
            .Where(x => !exclusions.Contains(x.ProsesApproval.StatusApproval));
    }

    protected IQueryable<WorkPaper> FilterXlsxRootCauses(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.ApprovalStatusExclusion is null)
        {
            return presaleData;
        }

        HashSet<string> exclusions = SessionService.FilterPreference.RootCauseExclusion.Exclusion;

        return presaleData.Where(x => (x.ProsesApproval.StatusApproval == ApprovalStatus.Reject
            || x.ProsesApproval.StatusApproval == ApprovalStatus.CloseLost)
            && !exclusions.Contains(x.ProsesApproval.RootCause, StringComparer.OrdinalIgnoreCase));
    }

    protected IQueryable<WorkPaper> FilterXlsxAgingImport(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.OperatorPacExclusionModel is null)
        {
            return presaleData;
        }

        HashSet<string> exclusions = SessionService.FilterPreference.OperatorPacExclusionModel.Exclusion;

        return presaleData.Where(x => !x.ApprovalOpportunity.SignatureVerifikasiImport.IsEmptySignature());
    }

    public async Task OpenBoundaryFilterDialogAsync()
    {
        var boundary = SessionService.FilterPreference.BoundaryFilters[ActiveTabId];

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

        if (result.Cancelled || result.Data == null)
        {
            SessionService.FilterPreference.UpperBoundaryDateTimeMin = new DateTime(upperBoundaryMin.Ticks);
            SessionService.FilterPreference.UpperBoundaryDateTimeMax = new DateTime(upperBoundaryMax.Ticks);
            SessionService.FilterPreference.MiddleBoundaryDateTimeMin = new DateTime(middleBoundaryMin.Ticks);
            SessionService.FilterPreference.MiddleBoundaryDateTimeMax = new DateTime(middleBoundaryMax.Ticks);
            SessionService.FilterPreference.LowerBoundaryDateTime = new DateTime(lowerBoundary.Ticks);

            return;
        }

        switch (boundary)
        {
            case BoundaryFilterMode.Monthly:
            {
                await ReloadUpperBoundaryAsync();
                break;
            }
            case BoundaryFilterMode.Weekly:
            {
                await ReloadMiddleBoundaryAsync();
                break;
            }
            case BoundaryFilterMode.Daily:
            {
                await ReloadLowerBoundaryAsync();
                break;
            }
            default:
                break;
        }
    }

    public async Task ApplyRootCauseExclusionFiltersAsync()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public async Task ApplyApprovalStatusExclusionFiltersAsync()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public async Task ApplyOperatorExclusionFiltersAsync()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public void OnActiveTabIdChanged(string tabId)
    {
        ActiveTabId = tabId;
        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);
    }

    public async Task ReloadUpperBoundaryAsync()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await ResetUpperBoundary();

        GenerateStatusApprovalReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateRootCauseReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateImportAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateVerificationAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateChatCallMulaiAgingReports(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateChatCallResponsAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);
        GenerateApprovalAgingReport(includeUpper: true, includeMiddle: true, includeLower: true);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public async Task ReloadMiddleBoundaryAsync()
    {
        ResetMiddleBoundary();

        GenerateStatusApprovalReports(includeMiddle: true);
        GenerateRootCauseReports(includeMiddle: true);
        GenerateImportAgingReports(includeMiddle: true);
        GenerateVerificationAgingReports(includeMiddle: true);
        GenerateChatCallMulaiAgingReports(includeMiddle: true);
        GenerateChatCallResponsAgingReport(includeMiddle: true);
        GenerateApprovalAgingReport(includeMiddle: true);

        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();

        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);
    }

    public async Task ReloadLowerBoundaryAsync()
    {
        ResetLowerBoundary();

        GenerateStatusApprovalReports(includeLower: true);
        GenerateRootCauseReports(includeLower: true);
        GenerateImportAgingReports(includeLower: true);
        GenerateVerificationAgingReports(includeLower: true);
        GenerateChatCallMulaiAgingReports(includeLower: true);
        GenerateChatCallResponsAgingReport(includeLower: true);
        GenerateApprovalAgingReport(includeLower: true);

        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();

        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);
    }

    private IQueryable<WorkPaper>? GetBoundaryPresaleData(BoundaryFilterMode boundary)
    {
        switch (boundary)
        {
            case BoundaryFilterMode.Monthly:
                return UpperBoundaryPresaleData;
            case BoundaryFilterMode.Weekly:
                return MiddleBoundaryPresaleData;
            case BoundaryFilterMode.Daily:
                return LowerBoundaryPresaleData;
            default:
                throw new NotImplementedException();;
        }
    }
}
