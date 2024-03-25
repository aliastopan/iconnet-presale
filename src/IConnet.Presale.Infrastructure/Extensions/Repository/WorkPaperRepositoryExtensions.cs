using IConnet.Presale.Domain.Aggregates.Presales;
using Microsoft.EntityFrameworkCore;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class WorkPaperRepositoryExtensions
{
    public static async Task<WorkPaper?> FindWorkPaperAsync(this AppDbContext context, string idPermohonan)
    {
        return await context.WorkPapers
            // .Include(x => x.ApprovalOpportunity)
            .FirstOrDefaultAsync(x => x.ApprovalOpportunity.IdPermohonan == idPermohonan);
    }
}
