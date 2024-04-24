namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ChatCallResponsAgingReportModel
{
    private readonly bool _isWinning;

    public ChatCallResponsAgingReportModel(Guid helpdeskId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int chatCallResponsTotal, int slaWonTotal, bool isWinning)
    {
        HelpdeskId = helpdeskId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ChatCallResponsTotal = chatCallResponsTotal;
        SlaWonTotal = slaWonTotal;
        _isWinning = isWinning;
        SlaWinRate = slaWonTotal / (float)chatCallResponsTotal * 100;
    }

    public Guid HelpdeskId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ChatCallResponsTotal { get; init; }
    public int SlaWonTotal { get; init; }
    public int SlaLostTotal => ChatCallResponsTotal - SlaWonTotal;
    public bool IsWinning => _isWinning && ChatCallResponsTotal > 0;
    public float SlaWinRate { get; init; }

    public string GetDisplayAverageAging()
    {
        return ChatCallResponsTotal > 0
            ? Average.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMinAging()
    {
        return ChatCallResponsTotal > 0
            ? Min.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMaxAging()
    {
        return ChatCallResponsTotal > 0
            ? Max.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayChatCallResponsTotal()
    {
        return ChatCallResponsTotal > 0
            ? ChatCallResponsTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWonTotal()
    {
        return ChatCallResponsTotal > 0
            ? SlaWonTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaLostTotal()
    {
        return ChatCallResponsTotal > 0
            ? SlaLostTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWinRate()
    {
        return ChatCallResponsTotal > 0
            ? $"{SlaWinRate:F2}%"
            : "Kosong";
    }

    public string GetSlaVerdict()
    {
        if (ChatCallResponsTotal <= 0)
        {
            return "Kosong";
        }

        return IsWinning
            ? "Win"
            : "Lose";
    }
}
