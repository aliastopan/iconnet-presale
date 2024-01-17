namespace IConnet.Presale.Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
}
