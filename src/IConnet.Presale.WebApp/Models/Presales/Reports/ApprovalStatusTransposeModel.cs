namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalStatusTransposeModel
{
    public string Office { get; set; } = default!;
    public Dictionary<ApprovalStatus, int> ApprovalStatusMetrics { get; set; } = default!;
}
