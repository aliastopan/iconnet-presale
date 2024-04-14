using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalAgingTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<ApprovalAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }
}
