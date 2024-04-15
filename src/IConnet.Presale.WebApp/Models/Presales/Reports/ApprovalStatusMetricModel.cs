namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalStatusMetricModel
{
    public string Office { get; set; } = default!;
    public Dictionary<ApprovalStatus, int> Status { get; set; } = default!;
}
