namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
    DateTime Zero { get; }

    string GetStringDateToday();
    long GetUnixTime(DateTime dateTime);
    string GetClockTime();
    string GetFormat();
    string[] GetParsingFormat();
    TimeSpan GetElapsedTime(DateTime startDateTime);
    DateTime ParseExact(string dateTimeString);
    bool IsToday(DateTime dateTime);
    bool IsWithinCurrentWeek(DateTime dateTimeMin, DateTime dateTimeMax);
    bool IsWithinCurrentMonth(DateTime dateTimeMin, DateTime dateTimeMax);
    int GetCurrentWeekOfYear();
    int GetCurrentWeekOfMonth();
    int GetWeekOfMonth(DateTime dateTime);
    int GetWeekOfYear(DateTime dateTime);
    string GetTimeIdentifier();
}
