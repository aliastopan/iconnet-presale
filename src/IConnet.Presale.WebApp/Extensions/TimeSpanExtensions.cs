namespace IConnet.Presale.WebApp.Extensions;

public static class TimeSpanExtensions
{
    public static string ToReadableFormat(this TimeSpan timeSpan)
    {
        int seconds = (int)timeSpan.TotalSeconds;

        int hours = seconds / 3600;
        seconds %= 3600;
        int minutes = seconds / 60;
        seconds %= 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    public static string ToReadableDateTime(this TimeSpan timeSpan)
    {
        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;

        if (days >= 365)
        {
            return "Lebih dari 1 tahun";
        }

        if (timeSpan.TotalMinutes < 1)
        {
            return "Kurang dari 1 menit";
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
}
