namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
    DateTime Zero { get; }

    string GetClockTime();
    string GetFormat();
    TimeSpan GetElapsedTime(DateTime startDateTime);
    DateTime ParseExact(string dateTimeString);
    int GetCurrentWeekOfYear();
    string GetTimeIdentifier();
}
