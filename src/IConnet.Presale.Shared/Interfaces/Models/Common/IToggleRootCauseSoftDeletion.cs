namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IToggleRootCauseSoftDeletion
{
    public Guid RootCauseId { get; }
    public bool IsDeleted { get; }
}
