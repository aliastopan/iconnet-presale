#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class Coordinate
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    [NotMapped]
    public string LatitudeLongitude => $"{Latitude}, {Longitude}";
}
