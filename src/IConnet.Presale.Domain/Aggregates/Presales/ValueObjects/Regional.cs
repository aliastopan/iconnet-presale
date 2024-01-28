#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Regional : ValueObject
{
    public Regional()
    {
        Koordinat = new Coordinate();
    }

    public Regional(string bagian, string kantorPerwakilan, string provinsi,
        string kabupaten, string kecamatan, string kelurahan, Coordinate koordinat)
    {
        Bagian = bagian;
        KantorPerwakilan = kantorPerwakilan;
        Provinsi = provinsi;
        Kabupaten = kabupaten;
        Kecamatan = kecamatan;
        Kelurahan = kelurahan;
        Koordinat = koordinat;
    }

    public string Bagian { get; set; }
    public string KantorPerwakilan { get; set; }
    public string Provinsi { get; set; }
    public string Kabupaten { get; set; }
    public string Kecamatan { get; set; }
    public string Kelurahan { get; set; }
    public Coordinate Koordinat { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Bagian;
        yield return KantorPerwakilan;
        yield return Provinsi;
        yield return Kabupaten;
        yield return Kecamatan;
        yield return Kelurahan;
        yield return Koordinat;
    }
}
