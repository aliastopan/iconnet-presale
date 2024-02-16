#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationParameter : ValueObject
{
    public ValidationParameter()
    {

    }

    public ValidationParameter(ValidationStatus validasiIdPln, ValidationStatus validasiNama, ValidationStatus validasiNomorTelepon,
        ValidationStatus validasiEmail, ValidationStatus validasiAlamat, Coordinate shareLoc)
    {
        ValidasiIdPln = validasiIdPln;
        ValidasiNama = validasiNama;
        ValidasiNomorTelepon = validasiNomorTelepon;
        ValidasiEmail = validasiEmail;
        ValidasiAlamat = validasiAlamat;
        ShareLoc = shareLoc;
    }

    public ValidationStatus ValidasiIdPln { get; init; } = default;
    public ValidationStatus ValidasiNama { get; init; } = default;
    public ValidationStatus ValidasiNomorTelepon { get; init; } = default;
    public ValidationStatus ValidasiEmail { get; init; } = default;
    public ValidationStatus ValidasiAlamat { get; init; } = default;
    public Coordinate ShareLoc { get; init; } = new();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ValidasiIdPln;
        yield return ValidasiNama;
        yield return ValidasiNomorTelepon;
        yield return ValidasiEmail;
        yield return ValidasiAlamat;
        yield return ShareLoc;
    }
}
