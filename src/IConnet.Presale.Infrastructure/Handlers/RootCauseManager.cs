using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Handlers;

internal class RootCauseHandler : IRootCauseHandler
{
    private readonly AppDbContextFactory _dbContextFactory;

    public RootCauseHandler(AppDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Result<ICollection<RootCause>> TryGetRootCauses()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var rootCauses = dbContext.GetRootCauses();
        if (rootCauses is null || rootCauses.Count == 0)
        {
            var error = new Error($" Not found.", ErrorSeverity.Error);
            return Result<ICollection<RootCause>>.NotFound(error);
        }

        return Result<ICollection<RootCause>>.Ok(rootCauses);
    }
}
