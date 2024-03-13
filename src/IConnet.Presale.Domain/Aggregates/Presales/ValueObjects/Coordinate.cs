#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Coordinate : ValueObject
{
    public Coordinate()
    {

    }

    public Coordinate(string latitudeLongitude)
    {
        var coordinates = latitudeLongitude.Split(',');
        if (coordinates.Length ==  2)
        {
            Latitude = coordinates[0].Trim();
            Longitude = coordinates[1].Trim();
        }
        else
        {
            throw new ArgumentException("Invalid latitude and longitude format.", nameof(latitudeLongitude));
        }
    }

    public Coordinate(string latitude, string longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public string Latitude { get; init; } = string.Empty;
    public string Longitude { get; init; } = string.Empty;

    public string GetLatitudeLongitude()
    {
        if (Latitude == string.Empty || Longitude == string.Empty)
        {
            return string.Empty;
        }

        return $"{Latitude}, {Longitude}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
