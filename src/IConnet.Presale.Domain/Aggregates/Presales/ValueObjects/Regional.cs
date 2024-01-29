#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Regional : ValueObject
{
    public Regional()
    {

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

    public string Bagian { get; init; } = string.Empty;
    public string KantorPerwakilan { get; init; } = string.Empty;
    public string Provinsi { get; init; } = string.Empty;
    public string Kabupaten { get; init; } = string.Empty;
    public string Kecamatan { get; init; } = string.Empty;
    public string Kelurahan { get; init; } = string.Empty;
    public Coordinate Koordinat { get; init; } = new();

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
