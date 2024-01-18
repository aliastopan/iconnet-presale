namespace IConnet.Presale.Shared.Interfaces.Models.Presales;

public interface IApprovalOpportunityModel
{
    string IdPermohonan { get; }
    string TglPermohonan { get; }
    string DurasiTidakLanjut { get; }
    string NamPemohon { get; }
    string IdPln { get; }
    string Layanan { get; }
    string SumberPemohon { get; }
    string StatusPermohonan { get; }
    string NamaAgen { get; }
    string EmailAgen { get; }
    string TeleponAgen { get; }
    string MitraAgen { get; }
    string Splitter { get; }
    string JenisPermohonan { get; }
    string TeleponPemohon { get; }
    string EmailPemohon { get; }
    string NikPemohon { get; }
    string NpwpPemohon { get; }
    string Keterangan { get; }
    string AlamatPemohon { get; }
    string Regional { get; }
    string KantorPerwakilan { get; }
    string Provinsi { get; }
    string Kabupaten { get; }
    string Kecamatan { get; }
    string Kelurahan { get; }
    string Latitude { get; }
    string Longitude { get; }
}
