namespace IConnet.Presale.WebApp.Extensions;

public static class TimeOnlyExtensions
{
    public static string ToClock(this TimeOnly time)
    {
        return $"{time.Hour:D2}:{time.Minute:D2}";
    }
}
