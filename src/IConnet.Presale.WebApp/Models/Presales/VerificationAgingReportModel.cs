namespace IConnet.Presale.WebApp.Models.Presales;

public class VerificationAgingReportModel
{
    public VerificationAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int verificationCount)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        VerificationCount = verificationCount;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int VerificationCount { get; init; }

    public string GetDisplayAverageAging()
    {
        return VerificationCount > 0
            ? Average.ToReadableFormat()
            : "Tidak Pernah Verifikasi";
    }

    public string GetDisplayMinAging()
    {
        return VerificationCount > 0
            ? Min.ToReadableFormat()
            : "Tidak Pernah Verifikasi";
    }

    public string GetDisplayMaxAging()
    {
        return VerificationCount > 0
            ? Max.ToReadableFormat()
            : "Tidak Pernah Verifikasi";
    }
    public string GetDisplayVerificationCount()
    {
        return VerificationCount > 0
            ? VerificationCount.ToString()
            : "Tidak Pernah Verifikasi";
    }
}
