namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class VerificationAgingReportModel
{
    public VerificationAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int verificationTotal, int totalReject, int totalVerified)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        VerificationTotal = verificationTotal;
        TotalReject = totalReject;
        TotalVerified = totalVerified;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int VerificationTotal { get; init; }
    public int TotalReject { get; init; }
    public int TotalVerified { get; init; }

    public string GetDisplayAverageAging()
    {
        return VerificationTotal > 0
            ? Average.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMinAging()
    {
        return VerificationTotal > 0
            ? Min.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMaxAging()
    {
        return VerificationTotal > 0
            ? Max.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayVerificationTotal()
    {
        return VerificationTotal > 0
            ? VerificationTotal.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalReject()
    {
        return VerificationTotal > 0
            ? TotalReject.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalVerified()
    {
        return VerificationTotal > 0
            ? TotalVerified.ToString()
            : "Kosong";
    }
}
