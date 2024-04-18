using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class RootCauseTabulationStackBase : ReportTabulationStackBase
{
    [Inject] public OptionService OptionService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<RootCauseReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> LowerBoundaryModels { get; set; } = [];

    public List<RootCauseReportModel> SortedUpperBoundaryModels => UpperBoundaryModels.Where(x => x.GrandTotal > 0).ToList();

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    public bool IsPageView { get; set; }

    protected async Task OpenRootCauseExclusionDialogFilter()
    {
        await FilterAsync();
    }

    private async Task FilterAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Root Causes Exclusion Filters",
            TrapFocus = true,
            Width = "500px",
        };

        var exclusion = SessionService.FilterPreference.RootCauseExclusion;
        var dialog = await DialogService.ShowDialogAsync<RootCauseExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (RootCauseExclusionModel)result.Data;

        SessionService.FilterPreference.RootCauseExclusion = dialogData;

        if (OnExclusionFilter.HasDelegate)
        {
            await OnExclusionFilter.InvokeAsync();
        }
    }

}
