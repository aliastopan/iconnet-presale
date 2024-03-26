#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationCorrection : ValueObject
{
    public ValidationCorrection()
    {
        PembetulanIdPln = string.Empty;
        PembetulanNama = string.Empty;
        PembetulanNomorTelepon = string.Empty;
        PembetulanEmail  = string.Empty;
        PembetulanAlamat = string.Empty;
    }

    public ValidationCorrection(string idPln, string namaPelanggan,
        string nomorTelepon, string email, string alamat)
    {
        PembetulanIdPln = idPln;
        PembetulanNama = namaPelanggan;
        PembetulanNomorTelepon = nomorTelepon;
        PembetulanEmail = email;
        PembetulanAlamat = alamat;
    }

    public string PembetulanIdPln { get; init; } = string.Empty;
    public string PembetulanNama { get; init; } = string.Empty;
    public string PembetulanNomorTelepon { get; init; } = string.Empty;
    public string PembetulanEmail { get; init; } = string.Empty;
    public string PembetulanAlamat { get; init; } = string.Empty;

    public ValidationCorrection Pembetulan(string propertyName, string pembetulan)
    {
        var methodMap = new Dictionary<string, Func<string, ValidationCorrection>>
        {
            { ValidationCorrectionPropertyNames.PembetulanIdPln, WithPembetulanIdPln },
            { ValidationCorrectionPropertyNames.PembetulanNama, WithPembetulanNama },
            { ValidationCorrectionPropertyNames.PembetulanNomorTelepon, WithPembetulanNomorTelepon },
            { ValidationCorrectionPropertyNames.PembetulanEmail, WithPembetulanEmail },
            { ValidationCorrectionPropertyNames.PembetulanAlamat, WithPembetulanAlamat }
        };

        if (methodMap.TryGetValue(propertyName, out var setProperty))
        {
            return setProperty(pembetulan);
        }
        else
        {
            throw new ArgumentException($"Invalid field name: {propertyName}");
        }
    }

    public string GetPembetulan(string propertyName)
    {
        var propertyMap = new Dictionary<string, Func<string>>
        {
            { ValidationCorrectionPropertyNames.PembetulanIdPln, () => PembetulanIdPln },
            { ValidationCorrectionPropertyNames.PembetulanNama, () => PembetulanNama },
            { ValidationCorrectionPropertyNames.PembetulanNomorTelepon, () => PembetulanNomorTelepon },
            { ValidationCorrectionPropertyNames.PembetulanEmail, () => PembetulanEmail },
            { ValidationCorrectionPropertyNames.PembetulanAlamat, () => PembetulanAlamat }
        };

        if (propertyMap.TryGetValue(propertyName, out var pembetulan))
        {
            return pembetulan();
        }
        else
        {
            throw new ArgumentException($"Invalid field name: {propertyName}");
        }
    }

    public ValidationCorrection WithPembetulanIdPln(string pembetulanIdPln)
    {
        return new ValidationCorrection
        {
            PembetulanIdPln = pembetulanIdPln,
            PembetulanNama = this.PembetulanNama,
            PembetulanNomorTelepon = this.PembetulanNomorTelepon,
            PembetulanEmail = this.PembetulanEmail,
            PembetulanAlamat = this.PembetulanAlamat
        };
    }

    public ValidationCorrection WithPembetulanNama(string pembetulanNama)
    {
        return new ValidationCorrection
        {
            PembetulanIdPln = this.PembetulanIdPln,
            PembetulanNama = pembetulanNama,
            PembetulanNomorTelepon = this.PembetulanNomorTelepon,
            PembetulanEmail = this.PembetulanEmail,
            PembetulanAlamat = this.PembetulanAlamat
        };
    }

    public ValidationCorrection WithPembetulanNomorTelepon(string pembetulanNomorTelepon)
    {
        return new ValidationCorrection
        {
            PembetulanIdPln = this.PembetulanIdPln,
            PembetulanNama = this.PembetulanNama,
            PembetulanNomorTelepon = pembetulanNomorTelepon,
            PembetulanEmail = this.PembetulanEmail,
            PembetulanAlamat = this.PembetulanAlamat
        };
    }

    public ValidationCorrection WithPembetulanEmail(string pembetulanEmail)
    {
        return new ValidationCorrection
        {
            PembetulanIdPln = this.PembetulanIdPln,
            PembetulanNama = this.PembetulanNama,
            PembetulanNomorTelepon = this.PembetulanNomorTelepon,
            PembetulanEmail = pembetulanEmail,
            PembetulanAlamat = this.PembetulanAlamat
        };
    }

    public ValidationCorrection WithPembetulanAlamat(string pembetulanAlamat)
    {
        return new ValidationCorrection
        {
            PembetulanIdPln = this.PembetulanIdPln,
            PembetulanNama = this.PembetulanNama,
            PembetulanNomorTelepon = this.PembetulanNomorTelepon,
            PembetulanEmail = this.PembetulanEmail,
            PembetulanAlamat = pembetulanAlamat
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PembetulanIdPln;
        yield return PembetulanNama;
        yield return PembetulanNomorTelepon;
        yield return PembetulanEmail;
        yield return PembetulanAlamat;
    }
}

public static class ValidationCorrectionPropertyNames
{
    public static string PembetulanIdPln => nameof(ValidationCorrection.PembetulanIdPln);
    public static string PembetulanNama => nameof(ValidationCorrection.PembetulanNama);
    public static string PembetulanNomorTelepon => nameof(ValidationCorrection.PembetulanNomorTelepon);
    public static string PembetulanEmail => nameof(ValidationCorrection.PembetulanEmail);
    public static string PembetulanAlamat => nameof(ValidationCorrection.PembetulanAlamat);
}