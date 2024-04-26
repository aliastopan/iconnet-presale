namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IToggleRootCauseOptions
{
    public Guid RootCauseId { get; }
    public bool IsDeleted { get; }
    public bool IsOnVerification { get; }
}
