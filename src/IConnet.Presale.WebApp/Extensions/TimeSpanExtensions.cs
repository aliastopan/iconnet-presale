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

    public static string AsReadableDateTimeWithSeconds(this TimeSpan timeSpan)
    {
        int seconds = timeSpan.Seconds;
        int minutes = timeSpan.Minutes;
        int hours = timeSpan.Hours;

        if (timeSpan.TotalHours < 1)
        {
            if (timeSpan.TotalMinutes < 1)
            {
                return $"{seconds} Detik";
            }
            return $"{minutes} Menit {seconds} Detik";
        }

        return $"{hours} Jam {minutes} Menit {seconds} Detik";
    }
}
