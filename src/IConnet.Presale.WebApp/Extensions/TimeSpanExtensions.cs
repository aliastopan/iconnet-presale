namespace IConnet.Presale.WebApp.Extensions;

public static class TimeSpanExtensions
{
    public static string AsReadableDateTime(this TimeSpan timeSpan)
    {
        int minutes = timeSpan.Minutes;
        int hours = timeSpan.Hours;

        if (timeSpan.TotalHours < 1)
        {
            return $"{minutes} Menit";
        }

        return $"{hours} Jam {minutes} Menit";
    }
}