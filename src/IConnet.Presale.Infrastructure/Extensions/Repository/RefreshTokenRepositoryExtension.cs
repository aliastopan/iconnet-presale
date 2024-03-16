using IConnet.Presale.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class RefreshTokenRepositoryExtensions
{
    public static async Task<List<RefreshToken>> GetRefreshTokensByUserAccountIdAsync(this AppDbContext context, Guid userAccountId)
    {
        return await context.RefreshTokens.Where(x => x.FkUserAccountId == userAccountId).ToListAsync();
    }

    public static RefreshToken? GetRefreshToken(this AppDbContext context, string token)
    {
        return context.RefreshTokens
            .SingleOrDefault(x => x.Token == token);
    }

    public static List<RefreshToken> GetRefreshTokensBeforeDate(this AppDbContext context, DateTimeOffset beforeDateTime)
    {
        return context.RefreshTokens
            .Where(x => x.CreationDate < beforeDateTime)
            .ToList();
    }
}
