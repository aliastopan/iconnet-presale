#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Regional
{
    public Regional()
    {
        Koordinat = new Coordinate();
    }

    public string Bagian { get; set; }
    public string KantorPerwakilan { get; set; }
    public string Provinsi { get; set; }
    public string Kabupaten { get; set; }
    public string Kecamatan { get; set; }
    public string Kelurahan { get; set; }
    public Coordinate Koordinat { get; set; }
}
