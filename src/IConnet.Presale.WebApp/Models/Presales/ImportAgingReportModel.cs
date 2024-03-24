namespace IConnet.Presale.WebApp.Models.Presales;

public class ImportAgingReportModel
{
    public ImportAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int importCount)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ImportCount = importCount;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ImportCount { get; init; }

    public string GetDisplayAverageAging()
    {
        return ImportCount > 0
            ? Average.ToReadableFormat()
            : "Tidak Pernah Import";
    }

    public string GetDisplayMinAging()
    {
        return ImportCount > 0
            ? Min.ToReadableFormat()
            : "Tidak Pernah Import";
    }

    public string GetDisplayMaxAging()
    {
        return ImportCount > 0
            ? Max.ToReadableFormat()
            : "Tidak Pernah Import";
    }
    public string GetDisplayImportCount()
    {
        return ImportCount > 0
            ? ImportCount.ToString()
            : "Tidak Pernah Import";
    }
}
