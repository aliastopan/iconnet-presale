namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ImportAgingReportModel
{
    private readonly bool _isWinning;

    public ImportAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int importTotal, int slaWonTotal, bool isWinning)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ImportTotal = importTotal;
        SlaWonTotal = slaWonTotal;
        _isWinning = isWinning;
        SlaWinRate = slaWonTotal / (float)importTotal * 100;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ImportTotal { get; init; }
    public int SlaWonTotal { get; init; }
    public int SlaLostTotal => ImportTotal - SlaWonTotal;
    public bool IsWinning => _isWinning && ImportTotal > 0;
    public float SlaWinRate { get; init; }

    public string GetDisplayAverageAging()
    {
        return ImportTotal > 0
            ? Average.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMinAging()
    {
        return ImportTotal > 0
            ? Min.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMaxAging()
    {
        return ImportTotal > 0
            ? Max.ToReadableFormat()
            : "Kosong";
    }
    public string GetDisplayImportTotal()
    {
        return ImportTotal > 0
            ? ImportTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWonTotal()
    {
        return ImportTotal > 0
            ? SlaWonTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaLostTotal()
    {
        return ImportTotal > 0
            ? SlaLostTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWinRate()
    {
        return ImportTotal > 0
            ? $"{SlaWinRate:F2}%"
            : "Kosong";
    }

    public string GetSlaVerdict()
    {
        if (ImportTotal <= 0)
        {
            return "Kosong";
        }

        return IsWinning
            ? "Win"
            : "Lose";
    }
}
