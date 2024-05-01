namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class InProgressReportTransposeModel
{
    public string Office { get; set; } = default!;
    public Dictionary<WorkPaperLevel, int> WorkPaperLevelMetrics { get; set; } = default!;
}
