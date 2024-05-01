namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalStatusReportTransposeModel
{
    public string Office { get; set; } = default!;
    public Dictionary<ApprovalStatus, int> ApprovalStatusMetrics { get; set; } = default!;
}
