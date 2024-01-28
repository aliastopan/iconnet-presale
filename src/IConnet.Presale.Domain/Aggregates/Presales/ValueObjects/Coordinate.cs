#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Coordinate : ValueObject
{
    public Coordinate()
    {

    }

    public Coordinate(string latitude, string longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public string Latitude { get; init; }
    public string Longitude { get; init; }

    [NotMapped]
    public string LatitudeLongitude => $"{Latitude}, {Longitude}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
