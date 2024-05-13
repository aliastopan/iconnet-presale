using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class RootCauseClassificationTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<RootCauseClassificationReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseClassificationReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseClassificationReportModel> LowerBoundaryModels { get; set; } = [];
    [Parameter] public EventCallback ToggleRootCauseBreakdown { get; set; }

    public List<RootCauseClassificationReportTransposeModel> UpperBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(UpperBoundaryModels));
    public List<RootCauseClassificationReportTransposeModel> MiddleBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(MiddleBoundaryModels));
    public List<RootCauseClassificationReportTransposeModel> LowerBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(LowerBoundaryModels));

    public List<RootCauseClassificationReportModel> SortedUpperBoundaryModels => ReportService.SortRootCauseClassificationModel(UpperBoundaryModels);
    public List<RootCauseClassificationReportModel> SortedMiddleBoundaryModels => ReportService.SortRootCauseClassificationModel(MiddleBoundaryModels);
    public List<RootCauseClassificationReportModel> SortedLowerBoundaryModels => ReportService.SortRootCauseClassificationModel(LowerBoundaryModels);

    public Dictionary<string, List<RootCauseClassificationReportModel>> UpperBoundaryGrouping => ReportService.RootCauseClassificationBoundaryGrouping(SortedUpperBoundaryModels);
    public Dictionary<string, List<RootCauseClassificationReportModel>> MiddleBoundaryGrouping => ReportService.RootCauseClassificationBoundaryGrouping(SortedMiddleBoundaryModels);
    public Dictionary<string, List<RootCauseClassificationReportModel>> LowerBoundaryGrouping => ReportService.RootCauseClassificationBoundaryGrouping(SortedLowerBoundaryModels);

    public bool IsUpperBoundaryEmpty => UpperBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsMiddleBoundaryEmpty => MiddleBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsLowerBoundaryEmpty => LowerBoundaryModels.Sum(x => x.GrandTotal) == 0;

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    public bool IsPageView { get; set; }

    public async Task ToggleRootCauseBreakdownAsync()
    {
        if (ToggleRootCauseBreakdown.HasDelegate)
        {
            await ToggleRootCauseBreakdown.InvokeAsync();
        }
    }

    protected List<RootCauseClassificationReportModel> OrderByDescending(List<RootCauseClassificationReportModel> models)
    {
        return models.Where(x => x.GrandTotal > 0)
            .OrderByDescending(x => x.GrandTotal)
            .ToList();
    }
}
