#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Applicant : ValueObject
{
    public Applicant()
    {

    }

    public Applicant(string idPln, string namaLengkap, string nomorTelepon, string email,
        string alamat, string nik, string npwp, string keterangan)
    {
        IdPln = idPln;
        NamaLengkap = namaLengkap;
        NomorTelepon = nomorTelepon;
        Email = email;
        Alamat = alamat;
        Nik = nik;
        Npwp = npwp;
        Keterangan = keterangan;
    }

    public string IdPln { get; init; }
    public string NamaLengkap { get; init; }
    public string NomorTelepon { get; init; }
    public string Email { get; init; }
    public string Alamat { get; init; }
    public string Nik { get; init; }
    public string Npwp { get; init; }
    public string Keterangan { get; init; }

    [NotMapped]
    public string WhatsApp => $"wa.me/{(NomorTelepon.StartsWith('+') ? NomorTelepon[1..] : NomorTelepon)}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IdPln;
        yield return NamaLengkap;
        yield return NomorTelepon;
        yield return Email;
        yield return Alamat;
        yield return Nik;
        yield return Npwp;
        yield return Keterangan;
    }
}
