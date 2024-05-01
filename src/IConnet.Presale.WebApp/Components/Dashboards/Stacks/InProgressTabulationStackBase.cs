using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class InProgressTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<InProgressReportModel> WeeklyBoundaryModels { get; set; } = [];
    [Parameter] public List<InProgressReportModel> DailyBoundaryModels { get; set; } = [];

    public List<InProgressReportTransposeModel> WeeklyBoundaryTransposeModels => ReportService.TransposeModel(WeeklyBoundaryModels);
    public List<InProgressReportTransposeModel> DailyBoundaryTransposeModels => ReportService.TransposeModel(DailyBoundaryModels);

    public bool IsWeeklyBoundaryEmpty => WeeklyBoundaryModels.Sum(x => x.GrandTotal) == 0;
    public bool IsDailyBoundaryEmpty => DailyBoundaryModels.Sum(x => x.GrandTotal) == 0;

    protected override Appearance WeeklyToggleAppearance => IsWeeklySelected || IsMonthlySelected ? Appearance.Accent : Appearance.Neutral;
    protected override Appearance DailyToggleAppearance => base.DailyToggleAppearance;

    protected override string WeeklyToggleDisplayStyle => IsWeeklySelected || IsMonthlySelected ? "d-flex flex-column w-100" : "display: none";
    protected override string DailyToggleDisplayStyle => base.DailyToggleDisplayStyle;
}
