using System.Globalization;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
    public DateTime Zero => DateTime.MinValue;

    public long GetUnixTime(DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }

    public string GetClockTime()
    {
        return DateTimeOffset.Now.ToString("HH:mm");
    }

    public string GetFormat()
    {
        return "yyyy-MM-dd HH:mm";
    }

    public TimeSpan GetElapsedTime(DateTime startDateTime)
    {
        return DateTime.Now - startDateTime;
    }

    public DateTime ParseExact(string dateTimeString)
    {
        return DateTime.ParseExact(dateTimeString, GetFormat(), System.Globalization.CultureInfo.InvariantCulture);
    }

    public int GetCurrentWeekOfYear()
    {
        var now = DateTimeOffset.Now;
        var calendar = CultureInfo.CurrentCulture.Calendar;

        return calendar.GetWeekOfYear(now.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
    }

    public int GetWeekOfYear(DateTime dateTime)
    {
        var calendar = CultureInfo.CurrentCulture.Calendar;

        return calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
    }

    public int GetCurrentWeekOfMonth()
    {
        int dayOfMonth = DateTimeOffset.Now.Day;
        int weekOfMonth = (int)Math.Ceiling((double)dayOfMonth / 7);

        return weekOfMonth;
    }

    public int GetWeekOfMonth(DateTime dateTime)
    {
        int dayOfMonth = dateTime.Day;
        int weekOfMonth = (int)Math.Ceiling((double)dayOfMonth / 7);

        return weekOfMonth;
    }

    public string GetTimeIdentifier()
    {
        DateTime currentTime = DateTimeOffset.Now.LocalDateTime;
        int hour = currentTime.Hour;

        if (hour >= 5 && hour <= 10)
        {
            return "Pagi";
        }
        else if (hour >= 11 && hour <= 14)
        {
            return "Siang";
        }
        else if (hour >= 15 && hour <= 18)
        {
            return "Sore";
        }
        else
        {
            return "Malam";
        }
    }
}
