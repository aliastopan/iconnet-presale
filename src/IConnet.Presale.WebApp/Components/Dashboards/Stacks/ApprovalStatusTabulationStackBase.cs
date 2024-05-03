using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalStatusTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    public List<ApprovalStatusReportTransposeModel> UpperBoundaryTransposeModels => ReportService.TransposeModel(UpperBoundaryModels);
    public List<ApprovalStatusReportTransposeModel> MiddleBoundaryTransposeModels => ReportService.TransposeModel(MiddleBoundaryModels);
    public List<ApprovalStatusReportTransposeModel> LowerBoundaryTransposeModels => ReportService.TransposeModel(LowerBoundaryModels);

    public Dictionary<string, List<ApprovalStatusReportModel>> UpperBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(UpperBoundaryModels);
    public Dictionary<string, List<ApprovalStatusReportModel>> MiddleBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(MiddleBoundaryModels);
    public Dictionary<string, List<ApprovalStatusReportModel>> LowerBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(LowerBoundaryModels);

    public bool IsUpperBoundaryEmpty => UpperBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsMiddleBoundaryEmpty => MiddleBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsLowerBoundaryEmpty => LowerBoundaryModels.Sum(x => x.GrandTotal) == 0;

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    protected async Task OpenApprovalStatusExclusionDialogFilter()
    {
        var parameters = new DialogParameters()
        {
            Title = "Pilah Approval Status",
            TrapFocus = true,
            Width = "500px",
        };

        var exclusion = SessionService.FilterPreference.ApprovalStatusExclusion;
        var persistent = new ApprovalStatusExclusionModel(exclusion);
        var dialog = await DialogService.ShowDialogAsync<ApprovalStatusExclusionDialog>(exclusion, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            SessionService.FilterPreference.ApprovalStatusExclusion = persistent;
            return;
        }

        var dialogData = (ApprovalStatusExclusionModel)result.Data;

        SessionService.FilterPreference.ApprovalStatusExclusion = dialogData;

        if (OnExclusionFilter.HasDelegate)
        {
            await OnExclusionFilter.InvokeAsync();
        }
    }
}
