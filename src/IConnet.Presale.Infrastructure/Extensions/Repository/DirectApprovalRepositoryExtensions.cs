using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class DirectApprovalRepositoryExtensions
{
    public static List<DirectApproval> GetDirectApprovals(this AppDbContext context)
    {
        return context.DirectApprovals
            .OrderBy(x => x.Order)
            .ToList();
    }
}
