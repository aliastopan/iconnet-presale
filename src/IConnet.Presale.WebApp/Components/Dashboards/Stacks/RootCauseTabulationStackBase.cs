using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class RootCauseTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected OptionService OptionService { get; set; } = default!;
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<RootCauseReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> LowerBoundaryModels { get; set; } = [];

    // public List<RootCauseReportModel> SortedUpperBoundaryModels => SortBoundaryModel(UpperBoundaryModels);
    // public List<RootCauseReportModel> SortedMiddleBoundaryModels => SortBoundaryModel(MiddleBoundaryModels);
    // public List<RootCauseReportModel> SortedLowerBoundaryModels => SortBoundaryModel(LowerBoundaryModels);

    public List<RootCauseReportTransposeModel> UpperBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(UpperBoundaryModels));
    public List<RootCauseReportTransposeModel> MiddleBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(MiddleBoundaryModels));
    public List<RootCauseReportTransposeModel> LowerBoundaryTransposeModels => ReportService.TransposeModel(OrderByDescending(LowerBoundaryModels));

    public List<RootCauseReportModel> AssortedUpperBoundaryModels => ReportService.SortRootCauseModels(UpperBoundaryModels);
    public List<RootCauseReportModel> AssortedMiddleBoundaryModels => ReportService.SortRootCauseModels(MiddleBoundaryModels);
    public List<RootCauseReportModel> AssortedLowerBoundaryModels => ReportService.SortRootCauseModels(LowerBoundaryModels);

    public bool IsUpperBoundaryEmpty => UpperBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsMiddleBoundaryEmpty => MiddleBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsLowerBoundaryEmpty => LowerBoundaryModels.Sum(x => x.GrandTotal) == 0;

    [Parameter] public EventCallback OnExclusionFilter { get; set; }

    public bool IsPageView { get; set; }

    protected List<RootCauseReportModel> OrderByDescending(List<RootCauseReportModel> models)
    {
        return models.Where(x => x.GrandTotal > 0)
            .OrderByDescending(x => x.GrandTotal)
            .ToList();
    }

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
