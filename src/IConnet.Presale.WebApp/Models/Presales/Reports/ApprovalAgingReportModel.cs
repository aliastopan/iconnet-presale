namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalAgingReportModel
{
    private readonly bool _isWinning;

    public ApprovalAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int approvalTotal, int totalCloseLost, int totalReject,
        int totalExpansion, int totalApprove, int totalDirectApproval,
        int slaWonTotal, bool isWinning)
    {
        PacId = pacId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ApprovalTotal = approvalTotal;
        TotalCloseLost = totalCloseLost;
        TotalReject = totalReject;
        TotalExpansion = totalExpansion;
        TotalApprove = totalApprove;
        TotalDirectApproval = totalDirectApproval;
        SlaWonTotal = slaWonTotal;
        _isWinning = isWinning;
        SlaWinRate = slaWonTotal / (float)approvalTotal * 100;
    }

    public Guid PacId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ApprovalTotal { get; init; }
    public int TotalCloseLost { get; init; }
    public int TotalReject { get; init; }
    public int TotalExpansion { get; init; }
    public int TotalApprove { get; init; }
    public int TotalDirectApproval { get; init; }
    public int SlaWonTotal { get; init; }
    public int SlaLostTotal => ApprovalTotal - SlaWonTotal;
    public bool IsWinning => _isWinning && ApprovalTotal > 0;
    public float SlaWinRate { get; init; }

    public string GetDisplayAverageAging()
    {
        return ApprovalTotal > 0
            ? Average.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMinAging()
    {
        return ApprovalTotal > 0
            ? Min.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMaxAging()
    {
        return ApprovalTotal > 0
            ? Max.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayApprovalTotal()
    {
        return ApprovalTotal > 0
            ? ApprovalTotal.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalCloseLost()
    {
        return ApprovalTotal > 0
            ? TotalCloseLost.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalReject()
    {
        return ApprovalTotal > 0
            ? TotalReject.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalExpansion()
    {
        return ApprovalTotal > 0
            ? TotalExpansion.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalApprove()
    {
        return ApprovalTotal > 0
            ? TotalApprove.ToString()
            : "Kosong";
    }

    public string GetDisplayTotalDirectApproval()
    {
        return ApprovalTotal > 0
            ? TotalDirectApproval.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWonTotal()
    {
        return ApprovalTotal > 0
            ? SlaWonTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaLostTotal()
    {
        return ApprovalTotal > 0
            ? SlaLostTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWinRate()
    {
        return ApprovalTotal > 0
            ? $"{SlaWinRate:F2}%"
            : "Kosong";
    }

    public string GetSlaVerdict()
    {
        if (ApprovalTotal <= 0)
        {
            return "Kosong";
        }

        return IsWinning
            ? "Win"
            : "Lose";
    }
}
