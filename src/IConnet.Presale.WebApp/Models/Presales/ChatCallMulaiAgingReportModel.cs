namespace IConnet.Presale.WebApp.Models.Presales;

public class ChatCallMulaiAgingReportModel
{
    public ChatCallMulaiAgingReportModel(Guid helpdeskId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int chatCallMulaiCount)
    {
        HelpdeskId = helpdeskId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ChatCallMulaiCount = chatCallMulaiCount;
    }

    public Guid HelpdeskId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ChatCallMulaiCount { get; init; }

    public string GetDisplayAverageAging()
    {
        return ChatCallMulaiCount > 0
            ? Average.ToReadableFormat()
            : "Tidak Pernah Chat/Call";
    }

    public string GetDisplayMinAging()
    {
        return ChatCallMulaiCount > 0
            ? Min.ToReadableFormat()
            : "Tidak Pernah Chat/Call";
    }

    public string GetDisplayMaxAging()
    {
        return ChatCallMulaiCount > 0
            ? Max.ToReadableFormat()
            : "Tidak Pernah Chat/Call";
    }
    public string GetDisplayChatCallMulaiCount()
    {
        return ChatCallMulaiCount > 0
            ? ChatCallMulaiCount.ToString()
            : "Tidak Pernah Chat/Call";
    }
}
