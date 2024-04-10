using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class RootCauseRepositoryExtension
{
    public static List<RootCause> GetRootCauses(this AppDbContext context)
    {
        return context.RootCauses
            .OrderBy(x => x.Order)
            .ToList();
    }

    public static RootCause? GetRootCause(this AppDbContext context, Guid rootCauseId)
    {
        return context.RootCauses.FirstOrDefault(x => x.RootCauseId == rootCauseId);
    }
}
