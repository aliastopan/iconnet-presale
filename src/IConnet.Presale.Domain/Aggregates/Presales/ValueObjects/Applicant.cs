#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Applicant : ValueObject
{
    public Applicant()
    {

    }

    public Applicant(string namaLengkap, string idPln, string email, string nomorTelepon,
        string nik, string npwp, string keterangan, string alamat)
    {
        NamaLengkap = namaLengkap;
        IdPln = idPln;
        Email = email;
        NomorTelepon = nomorTelepon;
        Nik = nik;
        Npwp = npwp;
        Keterangan = keterangan;
        Alamat = alamat;
    }

    public string NamaLengkap { get; init; }
    public string IdPln { get; init; }
    public string Email { get; init; }
    public string NomorTelepon { get; init; }
    public string Nik { get; init; }
    public string Npwp { get; init; }
    public string Keterangan { get; init; }
    public string Alamat { get; init; }

    [NotMapped]
    public string WhatsApp => $"wa.me/{(NomorTelepon.StartsWith('+') ? NomorTelepon[1..] : NomorTelepon)}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NamaLengkap;
        yield return IdPln;
        yield return Email;
        yield return NomorTelepon;
        yield return Nik;
        yield return Npwp;
        yield return Keterangan;
        yield return Alamat;
    }
}
