#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class WorkPaper : IAggregateRoot
{
    public WorkPaper()
    {
        ValidationParameter = new ValidationParameter();
    }

    public Guid WorkPaperId { get; set; }
    public string HelpdeskClaim { get; set; }
    public string HelpdeskShift { get; set; }
    public DateTimeOffset ChatCallDateTime { get; set; }
    public DateTimeOffset ResponseDateTime { get; set; }
    public string ChatHistoryRecapLink { get; set; }
    public ValidationParameter ValidationParameter { get; set; }
    public string ValidationStatus { get; set; }
    public string HelpdeskNote { get; set; }
    public string ApprovalStatus { get; set; }
    public string PacClaim { get; set; }
    public string ApprovalNote { get; set; }
    public string ShareLocDistance { get; set; }
    public string ICrmPlusDistance { get; set; }
    public DateTime VirtualAccountPublish { get; set; }

    public Guid FkApprovalOpportunityId { get; init; }
    public virtual ApprovalOpportunity ApprovalOpportunity { get; init; }
}

public class ValidationParameter
{
    public string PhoneNumber { get; set; }
    public string CustomerName { get; set; }
    public string EmailAddress { get; set; }
    public string CustomerAddress { get; set; }
    public string PlnId { get; set; }
    public string ShareLoc { get; set; }
}
