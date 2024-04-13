using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Handlers;

internal sealed class DirectApprovalHandler : IDirectApprovalHandler
{
    private readonly AppDbContextFactory _dbContextFactory;

    public DirectApprovalHandler(AppDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Result<ICollection<DirectApproval>> TryGetDirectApprovals()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var directApprovals = dbContext.GetDirectApprovals();
        if (directApprovals is null)
        {
            var error = new Error($"Direct Approval(s) not found.", ErrorSeverity.Warning);
            return Result<ICollection<DirectApproval>>.NotFound(error);
        }

        return Result<ICollection<DirectApproval>>.Ok(directApprovals);
    }

    public async Task AddDirectApprovalAsync(int order, string description)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var directApproval = new DirectApproval(order, description);

        dbContext.DirectApprovals.Add(directApproval);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Result> ToggleSoftDeletionAsync(Guid directApprovalId, bool isDeleted)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var directApproval = dbContext.GetDirectApproval(directApprovalId);

        if (directApproval is null)
        {
            var error = new Error("Direct Approval not found.", ErrorSeverity.Warning);
            return Result.NotFound(error);
        }

        directApproval.IsDeleted = isDeleted;
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}
