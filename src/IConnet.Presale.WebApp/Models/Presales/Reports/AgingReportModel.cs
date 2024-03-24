namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class AgingReportModel
{
    public AgingReportModel(TimeSpan average, TimeSpan min, TimeSpan max)
    {
        Average = average;
        Min = min;
        Max = max;
    }

    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
}
