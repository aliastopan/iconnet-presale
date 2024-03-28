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

    public static string ToReadableDateTime(this TimeSpan timeSpan, bool useLowerCaseNotation = false)
    {
        int days = timeSpan.Days;
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;

        if (days >= 365)
        {
            return !useLowerCaseNotation
                ? "Lebih dari 1 Tahun"
                : "lebih dari 1 tahun";
        }

        if (timeSpan.TotalMinutes < 1)
        {
            return !useLowerCaseNotation
                ? "Kurang dari 1 Menit"
                : "kurang dari 1 menit";
        }

        string result = "";
        string dayUnit = !useLowerCaseNotation ? "Hari" : "hari";
        string hourUnit = !useLowerCaseNotation ? "Jam" : "jam";
        string minuteUnit = !useLowerCaseNotation ? "Menit" : "menit";

        if (days > 0)
        {
            result += $"{days} {dayUnit} ";
        }

        if (hours > 0)
        {
            result += $"{hours} {hourUnit} ";
        }

        if (minutes > 0)
        {
            result += $"{minutes} {minuteUnit}";
        }
        else
        {
            result = result.TrimEnd();
        }

        return result;
    }
}
