#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationCorrection : ValueObject
{
    public ValidationCorrection()
    {

    }

    public ValidationCorrection(string idPln, string namaLengkap,
        string nomorTelepon, string email, string alamat)
    {
        IdPln = idPln;
        NamaLengkap = namaLengkap;
        NomorTelepon = nomorTelepon;
        Email = email;
        Alamat = alamat;
    }

    public string IdPln { get; init; } = string.Empty;
    public string NamaLengkap { get; init; } = string.Empty;
    public string NomorTelepon { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Alamat { get; init; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IdPln;
        yield return NamaLengkap;
        yield return NomorTelepon;
        yield return Email;
        yield return Alamat;
    }
}
