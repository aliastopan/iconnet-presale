#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationParameter : ValueObject
{
    public ValidationParameter()
    {

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

    public ValidationStatus PlnId { get; init; } = default;
    public ValidationStatus NamaPelanggan { get; init; } = default;
    public ValidationStatus NomorTelepon { get; init; } = default;
    public ValidationStatus Email { get; init; } = default;
    public ValidationStatus AlamatPelanggan { get; init; } = default;
    public Coordinate ShareLoc { get; init; } = new();

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
