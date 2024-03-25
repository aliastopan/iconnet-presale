#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class WorkPaper : IAggregateRoot, IConcurrencyTracking
{
    public WorkPaper()
    {
        WorkPaperId = Guid.NewGuid();
        ProsesValidasi = new ValidationProcess();
        ProsesApproval = new ApprovalProcess();
        ApprovalOpportunity = new ApprovalOpportunity();
    }

    public Guid WorkPaperId { get; init; }
    public WorkPaperLevel WorkPaperLevel { get; set; }
    public string Shift { get; set; }
    public ActionSignature SignatureHelpdeskInCharge { get; set; }
    public ActionSignature SignaturePlanningAssetCoverageInCharge { get; set; }
    public ValidationProcess ProsesValidasi { get; set; }
    public ApprovalProcess ProsesApproval { get; set; }

    public Guid FkApprovalOpportunityId { get; init; }
    public virtual ApprovalOpportunity ApprovalOpportunity { get; init; }

    [NotMapped]
    public DateTimeOffset LastModified { get; set; }

    public void SetHelpdeskInCharge(ActionSignature signatureHelpdeskInCharge)
    {
        SignatureHelpdeskInCharge = signatureHelpdeskInCharge;
    }

    public void SetPlanningAssetCoverageInCharge(ActionSignature signaturePlanningAssetCoverageInCharge)
    {
        SignaturePlanningAssetCoverageInCharge = signaturePlanningAssetCoverageInCharge;
    }

    public void ChangeSplitter(string splitter)
    {
        ApprovalOpportunity.Splitter = splitter;
    }

    public void UpdateWith(WorkPaper other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        WorkPaperLevel = other.WorkPaperLevel;
        Shift = other.Shift;
        SignatureHelpdeskInCharge = other.SignatureHelpdeskInCharge;
        SignaturePlanningAssetCoverageInCharge = other.SignaturePlanningAssetCoverageInCharge;
        ProsesValidasi = other.ProsesValidasi;
        ProsesApproval = other.ProsesApproval;
        LastModified = other.LastModified;
    }
}