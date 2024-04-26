using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.RootCauses.Commands.ToggleOptions;

public class ToggleRootCauseOptionsCommand : IToggleRootCauseOptions, IRequest<Result>
{
    public ToggleRootCauseOptionsCommand(Guid rootCauseId, bool isDeleted, bool isOnVerification)
    {
        RootCauseId = rootCauseId;
        IsDeleted = isDeleted;
        IsOnVerification = isOnVerification;
    }

    public Guid RootCauseId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsOnVerification { get; set; }
}
