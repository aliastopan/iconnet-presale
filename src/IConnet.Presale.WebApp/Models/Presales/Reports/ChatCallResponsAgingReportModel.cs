namespace IConnet.Presale.WebApp.Models.Presales.Reports;

public class ChatCallResponsAgingReportModel
{
    public ChatCallResponsAgingReportModel(Guid helpdeskId, string username,
        TimeSpan average, TimeSpan min, TimeSpan max, int chatCallResponsCount)
    {
        HelpdeskId = helpdeskId;
        Username = username;
        Average = average;
        Min = min;
        Max = max;
        ChatCallResponsCount = chatCallResponsCount;
    }

    public Guid HelpdeskId { get; init; }
    public string Username { get; init; }
    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }
    public int ChatCallResponsCount { get; init; }

    public string GetDisplayAverageAging()
    {
        return ChatCallResponsCount > 0
            ? Average.ToReadableFormat()
            : "Tidak Pernah Validasi";
    }

    public string GetDisplayMinAging()
    {
        return ChatCallResponsCount > 0
            ? Min.ToReadableFormat()
            : "Tidak Pernah Validasi";
    }

    public string GetDisplayMaxAging()
    {
        return ChatCallResponsCount > 0
            ? Max.ToReadableFormat()
            : "Tidak Pernah Validasi";
    }

    public string GetDisplayChatCallResponsCount()
    {
        return ChatCallResponsCount > 0
            ? ChatCallResponsCount.ToString()
            : "Tidak Pernah Validasi";
    }
}
