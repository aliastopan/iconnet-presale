using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalAgingTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<ApprovalAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalAgingReportModel> LowerBoundaryModels { get; set; } = [];

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
        var persistent = new OperatorExclusionModel(exclusion);
        var dialog = await DialogService.ShowDialogAsync<OperatorPacExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            SessionService.FilterPreference.OperatorPacExclusionModel = persistent;
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
