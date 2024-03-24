namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ImportAgingReportModel
{
    public ImportAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int importTotal)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ImportTotal = importTotal;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ImportTotal { get; init; }

    public string GetDisplayAverageAging()
    {
        return ImportTotal > 0
            ? Average.ToReadableFormat()
            : "Tidak Pernah Import";
    }

    public string GetDisplayMinAging()
    {
        return ImportTotal > 0
            ? Min.ToReadableFormat()
            : "Tidak Pernah Import";
    }

    public string GetDisplayMaxAging()
    {
        return ImportTotal > 0
            ? Max.ToReadableFormat()
            : "Tidak Pernah Import";
    }
    public string GetDisplayImportTotal()
    {
        return ImportTotal > 0
            ? ImportTotal.ToString()
            : "Tidak Pernah Import";
    }
}
