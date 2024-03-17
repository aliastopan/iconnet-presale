using IConnet.Presale.Domain.Aggregates.Presales;
using Microsoft.EntityFrameworkCore;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class ApprovalOpportunityRepositoryExtensions
{
    public static async Task<ApprovalOpportunity?> FindApprovalOpportunityAsync(this AppDbContext context, string idPermohonan)
    {
        return await context.ApprovalOpportunities
            .AsNoTracking()
            .FirstOrDefaultAsync(approvalOpportunity => approvalOpportunity.IdPermohonan == idPermohonan);
    }
}
