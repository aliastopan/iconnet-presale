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

    public ValidationParameter WithValidasiIdPln(ValidationStatus validasiIdPln)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = validasiIdPln,
            ValidasiNama = this.ValidasiNama,
            ValidasiNomorTelepon = this.ValidasiNomorTelepon,
            ValidasiEmail = this.ValidasiEmail,
            ValidasiAlamat = this.ValidasiAlamat,
            ShareLoc = this.ShareLoc
        };
    }

    public ValidationParameter WithValidasiNama(ValidationStatus validasiNama)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = this.ValidasiIdPln,
            ValidasiNama = validasiNama,
            ValidasiNomorTelepon = this.ValidasiNomorTelepon,
            ValidasiEmail = this.ValidasiEmail,
            ValidasiAlamat = this.ValidasiAlamat,
            ShareLoc = this.ShareLoc
        };
    }

    public ValidationParameter WithValidasiNomorTelepon(ValidationStatus validasiNomorTelepon)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = this.ValidasiIdPln,
            ValidasiNama = this.ValidasiNama,
            ValidasiNomorTelepon = validasiNomorTelepon,
            ValidasiEmail = this.ValidasiEmail,
            ValidasiAlamat = this.ValidasiAlamat,
            ShareLoc = this.ShareLoc
        };
    }


    public ValidationParameter WithValidasiEmail(ValidationStatus validasiEmail)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = this.ValidasiIdPln,
            ValidasiNama = this.ValidasiNama,
            ValidasiNomorTelepon = this.ValidasiNomorTelepon,
            ValidasiEmail = validasiEmail,
            ValidasiAlamat = this.ValidasiAlamat,
            ShareLoc = this.ShareLoc
        };
    }


    public ValidationParameter WithValidasiAlamat(ValidationStatus validasiAlamat)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = this.ValidasiIdPln,
            ValidasiNama = this.ValidasiNama,
            ValidasiNomorTelepon = this.ValidasiNomorTelepon,
            ValidasiEmail = this.ValidasiEmail,
            ValidasiAlamat = validasiAlamat,
            ShareLoc = this.ShareLoc
        };
    }


    public ValidationParameter WithShareLoc(Coordinate shareLoc)
    {
        return new ValidationParameter
        {
            ValidasiIdPln = this.ValidasiIdPln,
            ValidasiNama = this.ValidasiNama,
            ValidasiNomorTelepon = this.ValidasiNomorTelepon,
            ValidasiEmail = this.ValidasiEmail,
            ValidasiAlamat = this.ValidasiAlamat,
            ShareLoc = shareLoc
        };
    }

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
