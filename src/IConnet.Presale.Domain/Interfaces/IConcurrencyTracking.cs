namespace IConnet.Presale.Domain.Interfaces;

public interface IConcurrencyTracking
{
    DateTimeOffset LastModified { get; }
}
