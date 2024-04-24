using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ImportAgingTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<ImportAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ImportAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ImportAgingReportModel> LowerBoundaryModels { get; set; } = [];

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    public bool IsPageView { get; set; }

    protected async Task OpenOperatorPacExclusionDialogFilter()
    {
        await FilterAsync();
    }

    private async Task FilterAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Aging Import Exclusion Filters",
            TrapFocus = true,
            Width = "500px",
        };

        var exclusion = SessionService.FilterPreference.OperatorPacExclusionModel;
        var dialog = await DialogService.ShowDialogAsync<OperatorPacExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (OperatorPacExclusionModel)result.Data;

        SessionService.FilterPreference.OperatorPacExclusionModel = dialogData;

        if (OnExclusionFilter.HasDelegate)
        {
            await OnExclusionFilter.InvokeAsync();
        }
    }
}
