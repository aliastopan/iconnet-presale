using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IRepresentativeOfficeManager
{
    Result<ICollection<RepresentativeOffice>> TryGetRepresentativeOfficeAsync();
}
