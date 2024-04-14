using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class VerificationAgingTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<VerificationAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<VerificationAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<VerificationAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }
}
