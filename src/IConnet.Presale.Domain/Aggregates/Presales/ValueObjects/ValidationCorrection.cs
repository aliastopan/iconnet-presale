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

    public string IdPln { get; init; }
    public string NamaLengkap { get; init; }
    public string NomorTelepon { get; init; }
    public string Email { get; init; }
    public string Alamat { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IdPln;
        yield return NamaLengkap;
        yield return NomorTelepon;
        yield return Email;
        yield return Alamat;
    }
}
