#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class WorkPaper : IAggregateRoot
{
    public WorkPaper()
    {
        WorkPaperId = Guid.NewGuid();
        ProsesValidasi = new ValidationProcess();
        ProsesApproval = new ApprovalProcess();
    }

    public Guid WorkPaperId { get; set; }
    public string Shift { get; set; }
    public PersonInCharge PersonInCharge { get; set; }
    public ValidationProcess ProsesValidasi { get; set; }
    public ApprovalProcess ProsesApproval { get; set; }

    public Guid FkApprovalOpportunityId { get; init; }
    public virtual ApprovalOpportunity ApprovalOpportunity { get; init; }
}