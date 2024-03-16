using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class UserAccountRepositoryExtensions
{
    public static async Task<UserAccount?> GetUserAccountByIdAsync(this AppDbContext context, Guid userAccountId)
    {
        return await context.UserAccounts
            .FirstOrDefaultAsync(x => x.UserAccountId == userAccountId);
    }

    public static async Task<UserAccount?> GetUserAccountByUsernameAsync(this AppDbContext context, string username)
    {
        return await context.UserAccounts
            .FirstOrDefaultAsync(x => x.User.Username == username);
    }

    public static async Task<List<UserAccount>> GetUserAccountAsync(this AppDbContext context)
    {
        return await context.UserAccounts
            .ToListAsync();
    }

    public static async Task<List<UserAccount>> GetRangeUserAccountAsync(this AppDbContext context, int range = 0)
    {
        if (range > 0)
        {
            return await context.UserAccounts
                .Take(range)
                .ToListAsync();
        }
        else
        {
            return await context.UserAccounts
                .ToListAsync();
        }
    }
}
