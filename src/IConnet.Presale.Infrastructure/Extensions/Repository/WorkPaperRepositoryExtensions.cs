using IConnet.Presale.Domain.Aggregates.Presales;
using Microsoft.EntityFrameworkCore;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class WorkPaperRepositoryExtensions
{
    public static async Task<WorkPaper?> GetWorkPaper(this AppDbContext context, string idPermohonan)
    {
        return await context.WorkPapers
            .Include(workPaper => workPaper.ApprovalOpportunity)
            .FirstOrDefaultAsync(workPaper => workPaper.ApprovalOpportunity.IdPermohonan == idPermohonan);
    }
}
