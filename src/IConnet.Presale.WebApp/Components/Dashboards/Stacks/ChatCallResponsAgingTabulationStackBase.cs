using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ChatCallResponsAgingTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;

    [Parameter] public List<ChatCallResponsAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallResponsAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallResponsAgingReportModel> LowerBoundaryModels { get; set; } = [];

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
        var persistent = new OperatorExclusionModel(exclusion);
        var dialog = await DialogService.ShowDialogAsync<OperatorHelpdeskExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            SessionService.FilterPreference.OperatorHelpdeskExclusionModel = persistent;
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
