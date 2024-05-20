using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.RootCauses.Commands.EditRootCause;

public class EditRootCauseCommand : IEditRootCauseModel, IRequest<Result>
{
    public EditRootCauseCommand(Guid rootCauseId, string rootCause, string classification)
    {
        RootCauseId = rootCauseId;
        RootCause = rootCause;
        Classification = classification;
    }

    public Guid RootCauseId { get; set; }
    public string RootCause { get; set; }
    public string Classification { get; set; }
}
