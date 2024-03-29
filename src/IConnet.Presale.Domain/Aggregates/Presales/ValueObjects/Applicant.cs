#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Applicant : ValueObject
{
    public Applicant()
    {

    }

    public Applicant(string idPln, string namaPelanggan, string nomorTelepon, string email,
        string alamat, string nik, string npwp, string keterangan)
    {
        IdPln = idPln;
        NamaPelanggan = namaPelanggan;
        NomorTelepon = nomorTelepon;
        Email = email;
        Alamat = alamat;
        Nik = nik;
        Npwp = npwp;
        Keterangan = keterangan;
    }

    public string IdPln { get; init; } = string.Empty;
    public string NamaPelanggan { get; init; }  = string.Empty;
    public string NomorTelepon { get; init; }  = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Alamat { get; init; }  = string.Empty;
    public string Nik { get; init; } = string.Empty;
    public string Npwp { get; init; } = string.Empty;
    public string Keterangan { get; init; } = string.Empty;

    public string GetWhatsApp()
    {
        return $"wa.me/{(NomorTelepon.StartsWith('+') ? NomorTelepon[1..] : NomorTelepon)}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IdPln;
        yield return NamaPelanggan;
        yield return NomorTelepon;
        yield return Email;
        yield return Alamat;
        yield return Nik;
        yield return Npwp;
        yield return Keterangan;
    }

}
