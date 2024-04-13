using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.DirectApprovals.Commands.AddDirectApproval;

public class AddDirectApprovalCommand : IAddDirectApprovalModel, IRequest<Result>
{
    public AddDirectApprovalCommand(int order, string description)
    {
        Order = order;
        Description = description;
    }

    public int Order { get; set; }
    public string Description { get; set; }
}
