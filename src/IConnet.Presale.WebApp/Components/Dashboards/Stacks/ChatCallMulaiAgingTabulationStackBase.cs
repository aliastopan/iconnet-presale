using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ChatCallMulaiAgingTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<ChatCallMulaiAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallMulaiAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallMulaiAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    protected async Task OpenOperatorHelpdeskExclusionDialogFilterAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Pilah Helpdesk Username",
            TrapFocus = true,
            Width = "500px"
        };

        var exclusion = SessionService.FilterPreference.OperatorHelpdeskExclusionModel;
        var dialog = await DialogService.ShowDialogAsync<OperatorHelpdeskExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (OperatorExclusionModel)result.Data;

        SessionService.FilterPreference.OperatorHelpdeskExclusionModel = dialogData;

        if (OnExclusionFilter.HasDelegate)
        {
            await OnExclusionFilter.InvokeAsync();
        }
    }
}
