namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ChatCallMulaiAgingReportModel
{
    public ChatCallMulaiAgingReportModel(Guid helpdeskId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int chatCallMulaiTotal)
    {
        HelpdeskId = helpdeskId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ChatCallMulaiTotal = chatCallMulaiTotal;
    }

    public Guid HelpdeskId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ChatCallMulaiTotal { get; init; }

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
}
