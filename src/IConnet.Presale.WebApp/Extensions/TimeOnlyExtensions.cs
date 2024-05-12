namespace IConnet.Presale.WebApp.Extensions;

public static class TimeOnlyExtensions
{
    public static string ToClock(this TimeOnly time)
    {
        return $"{time.Hour:D2}:{time.Minute:D2}";
    }

    public static DateTime ToDateTimeFormat(this TimeOnly time)
    {
        return new DateTime(
            DateTime.Today.Year,
            DateTime.Today.Month,
            DateTime.Today.Day,
            time.Hour,
            time.Minute,
            time.Second,
            time.Millisecond);
    }
}
