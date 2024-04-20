using IConnet.Presale.WebApp.Components.Dashboards.Filters;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DashboardPageBase : MetricPageBase, IPageNavigation
{
    protected string ActiveTabId { get; set; } = "tab-1";
    protected bool IsBufferLoading => SessionService.FilterPreference.IsBufferLoading;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("dashboard-wip", PageNavName.Dashboard, PageRoute.Dashboard);
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

    public async Task OpenBoundaryFilterDialogAsync()
    {
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

        if (result.Cancelled || result.Data == null)
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

    public void ApplyRootCauseExclusionFilters()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        LogSwitch.Debug("Apply Filters");

        var exclusions = SessionService.FilterPreference.RootCauseExclusion.Exclusion;
        LogSwitch.Debug("Exclusion count {0}", exclusions.Count);

        foreach (var exclusion in exclusions)
        {
            LogSwitch.Debug("Exclude: {0}", exclusion);
        }

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public async Task ApplyApprovalStatusExclusionFiltersAsync()
    {
        SessionService.FilterPreference.IsBufferLoading = true;
        StateHasChanged();

        LogSwitch.Debug("Apply Filters");

        var exclusions = SessionService.FilterPreference.ApprovalStatusExclusion.Exclusion;
        LogSwitch.Debug("Exclusion count {0}", exclusions.Count);

        foreach (var exclusion in exclusions)
        {
            LogSwitch.Debug("Exclude: {0}", exclusion);
        }

        await Task.Delay(500);

        SessionService.FilterPreference.IsBufferLoading = false;
        StateHasChanged();
    }

    public void OnActiveTabIdChanged(string tabId)
    {
        ActiveTabId = tabId;
        SessionService.FilterPreference.RefreshBoundaryFilters(ActiveTabId);

        LogSwitch.Debug("Changing Tab: {0}", ActiveTabId);
    }

    // public async Task OnUpperBoundaryChangedAsync()
    // {
    //     LogSwitch.Debug("Checking new upper boundary");
    //     var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
    //     var upperBoundaryMax = SessionService.FilterPreference.UpperBoundaryDateTimeMax;
    //     var middleBoundaryMin = SessionService.FilterPreference.MiddleBoundaryDateTimeMin;
    //     var middleBoundaryMax = SessionService.FilterPreference.MiddleBoundaryDateTimeMax;
    //     var lowerBoundary = SessionService.FilterPreference.LowerBoundaryDateTime;

    //     LogSwitch.Debug("Upper Boundary Min {0}", upperBoundaryMin.Date);
    //     LogSwitch.Debug("Upper Boundary Max {0}", upperBoundaryMax.Date);
    //     LogSwitch.Debug("Middle Boundary Min {0}", middleBoundaryMin.Date);
    //     LogSwitch.Debug("Middle Boundary Max {0}", middleBoundaryMax.Date);
    //     LogSwitch.Debug("Lower Boundary {0}", lowerBoundary.Date);

    //     await Task.CompletedTask;
    // }

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
}
