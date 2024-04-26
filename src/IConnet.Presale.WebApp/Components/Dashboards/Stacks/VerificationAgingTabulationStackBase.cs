using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class VerificationAgingTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<VerificationAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<VerificationAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<VerificationAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    protected async Task OpenOperatorPacExclusionDialogFilterAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Pilah PAC Username",
            TrapFocus = true,
            Width = "500px"
        };

        var exclusion = SessionService.FilterPreference.OperatorPacExclusionModel;
        var dialog = await DialogService.ShowDialogAsync<OperatorPacExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (OperatorExclusionModel)result.Data;

        SessionService.FilterPreference.OperatorPacExclusionModel = dialogData;

        if (OnExclusionFilter.HasDelegate)
        {
            await OnExclusionFilter.InvokeAsync();
        }
    }
}
