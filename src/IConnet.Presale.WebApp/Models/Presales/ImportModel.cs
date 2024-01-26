#nullable disable
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.WebApp.Models.Presales;

public class ImportModel : IApprovalOpportunityModel
{
    public string IdPermohonan { get; set; }
    public string TglPermohonan { get; set; }
    public string DurasiTidakLanjut { get; set; }
    public string NamaPemohon { get; set; }
    public string IdPln { get; set; }
    public string Layanan { get; set; }
    public string SumberPermohonan { get; set; }
    public string StatusPermohonan { get; set; }
    public string NamaAgen { get; set; }
    public string EmailAgen { get; set; }
    public string TeleponAgen { get; set; }
    public string MitraAgen { get; set; }
    public string Splitter { get; set; }
    public string JenisPermohonan { get; set; }
    public string TeleponPemohon { get; set; }
    public string EmailPemohon { get; set; }
    public string NikPemohon { get; set; }
    public string NpwpPemohon { get; set; }
    public string Keterangan { get; set; }
    public string AlamatPemohon { get; set; }
    public string Regional { get; set; }
    public string KantorPerwakilan { get; set; }
    public string Provinsi { get; set; }
    public string Kabupaten { get; set; }
    public string Kecamatan { get; set; }
    public string Kelurahan { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    public DateTimeOffset TglImport { get; set; }
    public Guid ImportClaimAccountId { get; set; }
    public string ImportClaimAlias { get; set; }
}
