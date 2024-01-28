#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class Applicant
{
    public string NamaLengkap { get; set; }
    public string IdPln { get; set; }
    public string Email { get; set; }
    public string NomorTelepon { get; set; }
    public string Nik { get; set; }
    public string Npwp { get; set; }
    public string Keterangan { get; set; }
    public string Alamat { get; set; }

    [NotMapped]
    public string WhatsApp => $"wa.me/{(NomorTelepon.StartsWith('+') ? NomorTelepon[1..] : NomorTelepon)}";
}
