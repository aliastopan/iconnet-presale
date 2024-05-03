using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : MetricPageBase, IPageNavigation
{
    [Inject] WorksheetService WorksheetService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private ToastParameters<ProgressToastContent> _progressToastExporting = default!;

    protected string ActiveTabId { get; set; } = "tab-0";
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

    protected async Task ReloadDashboardAsync()
    {
        await ReloadUpperBoundaryAsync();
    }

    public async Task ExportXlsxAsync()
    {
        if (IsExportLoading)
        {
            return;
        }

        IsExportLoading = true;

        var tabId = Guid.NewGuid();

        ExportProgressToast(tabId);
        ToastService.ShowProgressToast(_progressToastExporting);

        await Task.Delay(500);

        var boundary = SessionService.FilterPreference.BoundaryFilters[ActiveTabId];
        var presaleData = GetBoundaryPresaleData(boundary);
        var username = SessionService.GetSessionAlias().ReplaceSpacesWithUnderscores();
        var dateLabel = DateTimeService.GetStringDateToday();

        if (presaleData is null)
        {
            return;
        }

        IQueryable<WorkPaper> exportTarget;
        byte[] xlsxBytes;
        string base64;
        string fileName;

        switch (ActiveTabId)
        {
            case "tab-0": // in-progress
            {
                break;
            }
            case "tab-1": // approval status
            {
                exportTarget = FilterXlsxStatusApprovals(presaleData);
                xlsxBytes = WorksheetService.GenerateStandardXlsxBytes(exportTarget, "Approval Status");
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_StatusApproval_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-2": // root cause
            {
                exportTarget = FilterXlsxRootCauses(presaleData);
                xlsxBytes = WorksheetService.GenerateStandardXlsxBytes(exportTarget, "Root Cause");
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_RootCause_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-3": // aging import
            {
                exportTarget = FilterXlsxAgingImport(presaleData);
                xlsxBytes = WorksheetService.GenerateAgingImportXlsxBytes(exportTarget);
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_AgingImport_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-4": // aging verifikasi
            {
                exportTarget = FilterXlsxAgingVerification(presaleData);
                xlsxBytes = WorksheetService.GenerateAgingVerificationXlsxBytes(exportTarget);
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_AgingVerifikasi_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-5": // aging chat/call pick-up
            {
                exportTarget = FilterXlsxAgingChatCallMulai(presaleData);
                xlsxBytes = WorksheetService.GenerateAgingChatCallMulaiXlsxBytes(exportTarget);
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_AgingPickUp_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-6": // aging chat/call validasi
            {
                exportTarget = FilterXlsxAgingChatCallRespons(presaleData);
                xlsxBytes = WorksheetService.GenerateAgingChatCallResponsXlsxBytes(exportTarget);
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_AgingValidasi_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            case "tab-7": // aging approval
            {
                exportTarget = FilterXlsxAgingApproval(presaleData);
                xlsxBytes = WorksheetService.GenerateAgingApprovalXlsxBytes(exportTarget);
                base64 = Convert.ToBase64String(xlsxBytes);
                fileName = $"Dashboard_AgingApproval_{username}_{dateLabel}.xlsx";

                await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);
                break;
            }
            default:
            {
                ToastService.CloseToast($"{tabId}");
                IsExportLoading = false;
                return;
            }
        }

        ToastService.CloseToast($"{tabId}");
        IsExportLoading = false;

        Log.Information("Export success.");
    }

    protected IQueryable<WorkPaper> FilterXlsxStatusApprovals(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.ApprovalStatusExclusion is null)
        {
            return presaleData;
        }

        HashSet<ApprovalStatus> exclusions = SessionService.FilterPreference.ApprovalStatusExclusion.Exclusion;

        return presaleData.Where(x => !exclusions.Contains(x.ProsesApproval.StatusApproval));
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

        HashSet<Guid> inclusionIds = SessionService.FilterPreference.OperatorPacExclusionModel.InclusionIds;

        return presaleData.Where(x => inclusionIds.Contains(x.ApprovalOpportunity.SignatureImport.AccountIdSignature));
    }

    protected IQueryable<WorkPaper> FilterXlsxAgingVerification(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.OperatorPacExclusionModel is null)
        {
            return presaleData;
        }

        HashSet<Guid> inclusionIds = SessionService.FilterPreference.OperatorPacExclusionModel.InclusionIds;

        return presaleData.Where(x => inclusionIds.Contains(x.ApprovalOpportunity.SignatureVerifikasiImport.AccountIdSignature));
    }

    protected IQueryable<WorkPaper> FilterXlsxAgingChatCallMulai(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.OperatorPacExclusionModel is null)
        {
            return presaleData;
        }

        HashSet<Guid> inclusionIds = SessionService.FilterPreference.OperatorHelpdeskExclusionModel.InclusionIds;

        return presaleData.Where(x => inclusionIds.Contains(x.ProsesValidasi.SignatureChatCallMulai.AccountIdSignature));
    }

    protected IQueryable<WorkPaper> FilterXlsxAgingChatCallRespons(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.OperatorPacExclusionModel is null)
        {
            return presaleData;
        }

        HashSet<Guid> inclusionIds = SessionService.FilterPreference.OperatorHelpdeskExclusionModel.InclusionIds;

        return presaleData.Where(x => inclusionIds.Contains(x.ProsesValidasi.SignatureChatCallRespons.AccountIdSignature));
    }

    protected IQueryable<WorkPaper> FilterXlsxAgingApproval(IQueryable<WorkPaper> presaleData)
    {
        if (SessionService.FilterPreference.OperatorPacExclusionModel is null)
        {
            return presaleData;
        }

        HashSet<Guid> inclusionIds = SessionService.FilterPreference.OperatorPacExclusionModel.InclusionIds;

        return presaleData.Where(x => inclusionIds.Contains(x.ProsesApproval.SignatureApproval.AccountIdSignature));
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

    private void ExportProgressToast(Guid tabId)
    {
        _progressToastExporting = new()
        {
            Id = $"{tabId}",
            Intent = ToastIntent.Download,
            Title = "Exporting",
            Timeout = 15000,
            Content = new ProgressToastContent()
            {
                Details = $"Memuat Laporan ke .xlsx",
            },
        };
    }
}
