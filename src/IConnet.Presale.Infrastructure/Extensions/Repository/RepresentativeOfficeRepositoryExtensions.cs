using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class RepresentativeOfficeRepositoryExtensions
{
    public static List<RepresentativeOffice> GetRepresentativeOffices(this AppDbContext context)
    {
        return context.RepresentativeOffices
            .OrderBy(x => x.Order)
            .ToList();
    }
}
