#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationParameter : ValueObject
{
    public ValidationParameter()
    {
        ShareLoc = new Coordinate();
    }

    public ValidationParameter(ValidationStatus plnId, ValidationStatus namaPelanggan, ValidationStatus nomorTelepon,
        ValidationStatus email, ValidationStatus alamatPelanggan, Coordinate shareLoc)
    {
        PlnId = plnId;
        NamaPelanggan = namaPelanggan;
        NomorTelepon = nomorTelepon;
        Email = email;
        AlamatPelanggan = alamatPelanggan;
        ShareLoc = shareLoc;
    }

    public ValidationStatus PlnId { get; init; }
    public ValidationStatus NamaPelanggan { get; init; }
    public ValidationStatus NomorTelepon { get; init; }
    public ValidationStatus Email { get; init; }
    public ValidationStatus AlamatPelanggan { get; init; }
    public Coordinate ShareLoc { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlnId;
        yield return NamaPelanggan;
        yield return NomorTelepon;
        yield return Email;
        yield return AlamatPelanggan;
        yield return ShareLoc;
    }
}
