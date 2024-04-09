using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.RootCauses.Commands;

public class AddRootCauseCommand : IAddRootCauseModel, IRequest<Result>
{
    public AddRootCauseCommand(int order, string cause)
    {
        Order = order;
        Cause = cause;
    }

    public int Order { get; set; }
    public string Cause { get; set; }
}
