#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ApprovalOpportunity
{
    public ApprovalOpportunity()
    {
        ApprovalOpportunityId = Guid.NewGuid();
        Pemohon = new Applicant();
        Agen = new Agent();
        Regional = new Regional();
    }

    public Guid ApprovalOpportunityId { get; set; }
    public string IdPermohonan { get; set; }
    public DateTime TglPermohonan { get; set; }
    public string StatusPermohonan { get; set; }
    public string Layanan { get; set; }
    public string SumberPermohonan { get; set; }
    public string JenisPermohonan { get; set; }
    public string Splitter { get; set; }

    public Applicant Pemohon { get; set; }
    public Agent Agen { get; set; }
    public Regional Regional { get; set; }

    [NotMapped] public TimeSpan OpportunityLifetime => DateTime.Now - TglPermohonan;

    public DateTime TglImport { get; set; }
    public string NamaClaimImport { get; set; }
}

public class Agent
{
    public string NamaLengkap { get; set; }
    public string Email { get; set; }
    public string NomorTelepon { get; set; }
    public string Mitra { get; set; }
}

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
}

public class Regional
{
    public Regional()
    {
        Koordinat = new Coordinate();
    }

    public string Bagian { get; set; }
    public string KantorPerwakilan { get; set; }
    public string Provinsi { get; set; }
    public string Kabupaten { get; set; }
    public string Kecamatan { get; set; }
    public string Kelurahan { get; set; }
    public Coordinate Koordinat { get; set; }
}

public class Coordinate
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }
}