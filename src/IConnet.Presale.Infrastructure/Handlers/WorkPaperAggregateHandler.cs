using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Handlers;

internal sealed class WorkPaperAggregateHandler : IWorkPaperAggregateHandler
{
    private readonly AppDbContextFactory _dbContextFactory;
    private readonly WorkPaperFactory _workPaperFactory;

    public WorkPaperAggregateHandler(AppDbContextFactory dbContextFactory,
        WorkPaperFactory workPaperFactory)
    {
        _dbContextFactory = dbContextFactory;
        _workPaperFactory = workPaperFactory;
    }

    public async Task<Result> TryInsertOrUpdateWorkPaperAsync(IWorkPaperModel workPaperModel)
    {
        try
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            WorkPaper? existingWorkPaper = await dbContext.FindWorkPaperAsync(workPaperModel.IdPermohonan);
            WorkPaper workPaper = _workPaperFactory.TransformWorkPaperFromModel(workPaperModel);

            if (existingWorkPaper is not null)
            {
                existingWorkPaper.UpdateWith(workPaper);

                dbContext.ApprovalOpportunities.Update(existingWorkPaper.ApprovalOpportunity);
                dbContext.WorkPapers.Update(existingWorkPaper);
                await dbContext.SaveChangesAsync();

                return Result.Ok();
            }
            else
            {
                dbContext.WorkPapers.Add(workPaper);
                await dbContext.SaveChangesAsync();

                return Result.Ok();
            }
        }
        catch (Exception exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Critical);

            return Result.Error(error);
        }
    }
}
