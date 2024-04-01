namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
    DateTime Zero { get; }

    long GetUnixTime(DateTime dateTime);
    string GetClockTime();
    string GetFormat();
    TimeSpan GetElapsedTime(DateTime startDateTime);
    DateTime ParseExact(string dateTimeString);
    int GetCurrentWeekOfYear();
    int GetCurrentWeekOfMonth();
    int GetWeekOfMonth(DateTime dateTime);
    int GetWeekOfYear(DateTime dateTime);
    string GetTimeIdentifier();
}
