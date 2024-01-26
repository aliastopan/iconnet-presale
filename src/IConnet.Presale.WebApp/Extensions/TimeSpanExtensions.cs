namespace IConnet.Presale.WebApp.Extensions;

public static class TimeSpanExtensions
{
    public static string AsReadableString(this TimeSpan timeSpan)
    {
        int minutes = timeSpan.Minutes;
        int hours = timeSpan.Hours;

        if (timeSpan.TotalHours < 1)
        {
            return $"{minutes} Menit";
        }

        string result = $"{hours} Jam {minutes} Menit";

        return result;
    }
}