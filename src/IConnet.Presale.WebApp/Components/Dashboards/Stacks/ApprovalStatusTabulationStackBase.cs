using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ApprovalStatusTabulationStackBase : ReportTabulationStackBase
{
    [Inject] ReportService ReportService { get; set;} = default!;

    [Parameter] public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    public List<ApprovalStatusMetricModel> UpperBoundaryMetrics => ReportService.ConvertToMetrics(UpperBoundaryModels);
    public List<ApprovalStatusMetricModel> MiddleBoundaryMetrics => ReportService.ConvertToMetrics(MiddleBoundaryModels);
    public List<ApprovalStatusMetricModel> LowerBoundaryMetrics => ReportService.ConvertToMetrics(LowerBoundaryModels);
}
