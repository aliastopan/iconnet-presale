using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class RootCauseExtension
{
    public static List<RootCause> GetRootCauses(this AppDbContext context)
    {
        return context.RootCauses
            .OrderBy(x => x.Order)
            .ToList();
    }
}
