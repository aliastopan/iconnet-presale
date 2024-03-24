namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ApprovalAgingReportModel
{
    public ApprovalAgingReportModel(Guid pacId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int approvalTotal, int totalCloseLost, int totalReject, int totalExpansion, int totalApprove)
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
}
