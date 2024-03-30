#nullable disable

namespace IConnet.Presale.Domain.Entities;

public class DirectApproval
{
    public DirectApproval()
    {

    }

    public DirectApproval(int order, string description)
    {
        Order = order;
        Description = description;
    }

    public Guid DirectApprovalId { get; set; }
    public int Order { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
}
