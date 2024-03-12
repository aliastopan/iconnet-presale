namespace IConnet.Presale.WebApp.Extensions;

public static class TimeSpanExtensions
{
    public static string AsReadableDateTime(this TimeSpan timeSpan)
    {
        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;

        if (days >= 365)
        {
            return "Lebih dari 1 tahun";
        }

        string result = "";

        if (days > 0)
        {
            result += $"{days} Hari ";
        }

        if (hours > 0)
        {
            result += $"{hours} Jam ";
        }

        if (minutes > 0)
        {
            result += $"{minutes} Menit";
        }
        else
        {
            result = result.TrimEnd();
        }

        return result;
    }

    public static string AsReadableDateTimeWithSeconds(this TimeSpan timeSpan)
    {
        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;

        if (days >= 365)
        {
            return "Lebih dari 1 tahun";
        }

        string result = "";

        if (days > 0)
        {
            result += $"{days} Hari ";
        }

        if (hours > 0)
        {
            result += $"{hours} Jam ";
        }

        if (minutes > 0)
        {
            result += $"{minutes} Menit ";
        }

        if (seconds > 0)
        {
            result += $"{seconds} Detik";
        }
        else
        {
            result = result.TrimEnd();
        }

        return result;
    }
}
