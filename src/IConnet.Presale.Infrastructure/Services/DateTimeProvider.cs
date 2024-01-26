namespace IConnet.Presale.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
    public DateTime Zero => DateTime.MinValue;

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
}
