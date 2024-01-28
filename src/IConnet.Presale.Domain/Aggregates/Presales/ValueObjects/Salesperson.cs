#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Salesperson : ValueObject
{
    public Salesperson()
    {

    }

    public Salesperson(string namaLengkap, string email, string nomorTelepon, string mitra)
    {
        NamaLengkap = namaLengkap;
        Email = email;
        NomorTelepon = nomorTelepon;
        Mitra = mitra;
    }

    public string NamaLengkap { get; init; }
    public string Email { get; init; }
    public string NomorTelepon { get; init; }
    public string Mitra { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NamaLengkap;
        yield return Email;
        yield return NomorTelepon;
        yield return Mitra;
    }
}
