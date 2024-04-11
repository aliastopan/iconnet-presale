using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.RootCauses.Commands.ToggleSoftDeletion;

public class ToggleRootCauseSoftDeletionCommand : IToggleRootCauseSoftDeletion, IRequest<Result>
{
    public ToggleRootCauseSoftDeletionCommand(Guid rootCauseId, bool isDeleted)
    {
        RootCauseId = rootCauseId;
        IsDeleted = isDeleted;
    }

    public Guid RootCauseId { get; set; }
    public bool IsDeleted { get; set; }
}
