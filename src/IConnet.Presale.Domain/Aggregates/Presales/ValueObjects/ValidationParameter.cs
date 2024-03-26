#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationParameter : ValueObject
{
    public ValidationParameter()
    {
        ValidasiIdPln = ValidationStatus.MenungguValidasi;
        ValidasiNama = ValidationStatus.MenungguValidasi;
        ValidasiNomorTelepon = ValidationStatus.MenungguValidasi;
        ValidasiEmail = ValidationStatus.MenungguValidasi;
        ValidasiAlamat = ValidationStatus.MenungguValidasi;
        ShareLoc = new Coordinate();
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

    public ValidationParameter Validasi(string propertyName, ValidationStatus validationStatus)
    {
        var methodMap = new Dictionary<string, Func<ValidationStatus, ValidationParameter>>
        {
            { ValidationParameterPropertyNames.ValidasiIdPln, WithValidasiIdPln },
            { ValidationParameterPropertyNames.ValidasiNama, WithValidasiNama },
            { ValidationParameterPropertyNames.ValidasiNomorTelepon, WithValidasiNomorTelepon },
            { ValidationParameterPropertyNames.ValidasiEmail, WithValidasiEmail },
            { ValidationParameterPropertyNames.ValidasiAlamat, WithValidasiAlamat },
        };

        if (methodMap.TryGetValue(propertyName, out var setProperty))
        {
            return setProperty(validationStatus);
        }
        else
        {
            throw new ArgumentException($"Invalid field name: {propertyName}");
        }
    }

    public ValidationStatus GetValidationStatus(string propertyName)
    {
        var propertyMap = new Dictionary<string, Func<ValidationStatus>>
        {
            { ValidationParameterPropertyNames.ValidasiIdPln, () => ValidasiIdPln },
            { ValidationParameterPropertyNames.ValidasiNama, () => ValidasiNama },
            { ValidationParameterPropertyNames.ValidasiNomorTelepon, () => ValidasiNomorTelepon },
            { ValidationParameterPropertyNames.ValidasiEmail, () => ValidasiEmail },
            { ValidationParameterPropertyNames.ValidasiAlamat, () => ValidasiAlamat },
        };

        if (propertyMap.TryGetValue(propertyName, out var validationStatus))
        {
            return validationStatus();
        }
        else
        {
            throw new ArgumentException($"Invalid field name: {propertyName}");
        }
    }

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

public static class ValidationParameterPropertyNames
{
    public static string ValidasiIdPln => nameof(ValidationParameter.ValidasiIdPln);
    public static string ValidasiNama => nameof(ValidationParameter.ValidasiNama);
    public static string ValidasiNomorTelepon => nameof(ValidationParameter.ValidasiNomorTelepon);
    public static string ValidasiEmail => nameof(ValidationParameter.ValidasiEmail);
    public static string ValidasiAlamat => nameof(ValidationParameter.ValidasiAlamat);
}