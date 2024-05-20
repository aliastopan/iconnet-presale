namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class RootCauseClassificationReportTransposeModel
{
    public string Office { get; init; } = default!;
    public Dictionary<string, int> ClassificationMetrics { get; init; } = default!;
}
