namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ChatCallMulaiAgingReportModel
{
    private readonly bool _isWinning;

    public ChatCallMulaiAgingReportModel(Guid helpdeskId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max,
        int chatCallMulaiTotal, int slaWonTotal, bool isWinning)
    {
        HelpdeskId = helpdeskId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ChatCallMulaiTotal = chatCallMulaiTotal;
        SlaWonTotal = slaWonTotal;
        _isWinning = isWinning;
        SlaWinRate = slaWonTotal / (float)chatCallMulaiTotal * 100;
    }

    public Guid HelpdeskId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ChatCallMulaiTotal { get; init; }
    public int SlaWonTotal { get; init; }
    public int SlaLostTotal => ChatCallMulaiTotal - SlaWonTotal;
    public bool IsWinning => _isWinning && ChatCallMulaiTotal > 0;
    public float SlaWinRate { get; init; }

    public string GetDisplayAverageAging()
    {
        return ChatCallMulaiTotal > 0
            ? Average.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMinAging()
    {
        return ChatCallMulaiTotal > 0
            ? Min.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayMaxAging()
    {
        return ChatCallMulaiTotal > 0
            ? Max.ToReadableFormat()
            : "Kosong";
    }

    public string GetDisplayChatCallMulaiTotal()
    {
        return ChatCallMulaiTotal > 0
            ? ChatCallMulaiTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWonTotal()
    {
        return ChatCallMulaiTotal > 0
            ? SlaWonTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaLostTotal()
    {
        return ChatCallMulaiTotal > 0
            ? SlaLostTotal.ToString()
            : "Kosong";
    }

    public string GetDisplaySlaWinRate()
    {
        return ChatCallMulaiTotal > 0
            ? $"{SlaWinRate:F2}%"
            : "Kosong";
    }

    public string GetSlaVerdict()
    {
        if (ChatCallMulaiTotal <= 0)
        {
            return "Kosong";
        }

        return IsWinning
            ? "Win"
            : "Lose";
    }
}
