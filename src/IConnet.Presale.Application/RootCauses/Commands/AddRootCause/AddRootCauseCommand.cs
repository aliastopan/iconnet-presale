using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.RootCauses.Commands.AddRootCause;

public class AddRootCauseCommand : IAddRootCauseModel, IRequest<Result>
{
    public AddRootCauseCommand(int order, string cause, string classification)
    {
        Order = order;
        Cause = cause;
        Classification = classification;
    }

    public int Order { get; set; }
    public string Cause { get; set; }
    public string Classification { get; set; }
}
