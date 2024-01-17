using IConnet.Presale.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace IConnet.Presale.Application.Common.Extensions.Repository;

public static class RefreshTokenExtensions
{
    public static async Task<List<RefreshToken>> GetRefreshTokensByUserAccountIdAsync(this IAppDbContext context, Guid userAccountId)
    {
        return await context.RefreshTokens.Where(x => x.FkUserAccountId == userAccountId).ToListAsync();
    }

    public static RefreshToken? GetRefreshToken(this IAppDbContext context, string token)
    {
        return context.RefreshTokens
            .Include(x => x.UserAccount)
                .ThenInclude(x => x.User)
            .Include(x => x.UserAccount)
                .ThenInclude(x => x.UserProfile)
            .SingleOrDefault(x => x.Token == token);
    }
}
