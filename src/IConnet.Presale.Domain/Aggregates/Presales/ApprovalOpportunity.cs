#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ApprovalOpportunity
{
    public ApprovalOpportunity()
    {
        ApprovalOpportunityId = Guid.NewGuid();
        Pemohon = new Applicant();
        Agen = new Salesperson();
        Regional = new Regional();
    }

    public Guid ApprovalOpportunityId { get; init; }
    public string IdPermohonan { get; set; }
    public DateTime TglPermohonan { get; set; }
    public string StatusPermohonan { get; set; }
    public string Layanan { get; set; }
    public string SumberPermohonan { get; set; }
    public string JenisPermohonan { get; set; }
    public string Splitter { get; set; }

    public Applicant Pemohon { get; set; }
    public Salesperson Agen { get; set; }
    public Regional Regional { get; set; }

    public ImportStatus StatusImport { get; set; }
    public ActionSignature SignatureImport { get; set ; }
    public ActionSignature SignatureVerifikasiImport { get; set ; }

    public TimeSpan GetOpportunityLifetime()
    {
        return DateTime.Now - TglPermohonan;
    }
}