using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalStatusTabulationStackBase : ReportTabulationStackBase
{
    [Inject] protected ReportService ReportService { get; set;} = default!;

    [Parameter] public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    public List<ApprovalStatusTransposeModel> UpperBoundaryTransposeModels => ReportService.TransposeModel(UpperBoundaryModels);
    public List<ApprovalStatusTransposeModel> MiddleBoundaryTransposeModels => ReportService.TransposeModel(MiddleBoundaryModels);
    public List<ApprovalStatusTransposeModel> LowerBoundaryTransposeModels => ReportService.TransposeModel(LowerBoundaryModels);

    public Dictionary<string, List<ApprovalStatusReportModel>> UpperBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(UpperBoundaryModels);
    public Dictionary<string, List<ApprovalStatusReportModel>> MiddleBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(MiddleBoundaryModels);
    public Dictionary<string, List<ApprovalStatusReportModel>> LowerBoundaryGrouping => ReportService.ApprovalStatusBoundaryGrouping(LowerBoundaryModels);
}
