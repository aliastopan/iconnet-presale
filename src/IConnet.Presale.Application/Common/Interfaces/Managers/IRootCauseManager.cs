using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IRootCauseManager
{
    Result<ICollection<RootCause>> TryRootCausesAsync();
}
