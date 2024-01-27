#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

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

    public Guid ApprovalOpportunityId { get; set; }
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

    [NotMapped]
    public TimeSpan OpportunityLifetime => DateTime.Now - TglPermohonan;

    public ImportStatus StatusImport { get; set; }
    public ActionSignature ImportSignature { get; set ; }
    public ActionSignature CrmVerificationSignature { get; set ; }
}