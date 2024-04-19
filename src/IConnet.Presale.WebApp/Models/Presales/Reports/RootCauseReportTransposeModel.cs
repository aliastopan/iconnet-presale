namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class RootCauseReportTransposeModel
{
    public string Office { get; init; } = default!;
    public Dictionary<string, int> RootCauses { get; init; } = default!;
}
