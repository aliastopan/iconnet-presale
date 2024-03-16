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

    public async Task<Result> TryInsertWorkPaperAsync(IWorkPaperModel workPaperModel)
    {
        try
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var duplicate = dbContext.GetWorkPaper(workPaperModel.IdPermohonan);

            if (duplicate is not null)
            {
                var error = new Error($"Duplicate {workPaperModel.IdPermohonan}", ErrorSeverity.Warning);

                return Result.Error(error);
            }

            var workPaper = _workPaperFactory.TransformWorkPaperFromModel(workPaperModel);

            dbContext.WorkPapers.Add(workPaper);
            await dbContext.SaveChangesAsync();

            return Result.Ok();

        }
        catch (Exception exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Critical);

            return Result.Error(error);
        }
    }
}