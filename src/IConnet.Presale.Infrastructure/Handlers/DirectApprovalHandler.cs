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
}
