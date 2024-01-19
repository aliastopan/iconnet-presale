namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
    DateTime Zero { get; }

    string GetFormat();
    DateTime ParseExact(string dateTimeString);
}
