namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IEditRootCauseModel
{
    public Guid RootCauseId { get; }
    public string RootCause { get; }
    public string Classification { get; }
}
