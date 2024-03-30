using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IRepresentativeOfficeHandler
{
    Result<ICollection<RepresentativeOffice>> TryGetRepresentativesOffices();
}
