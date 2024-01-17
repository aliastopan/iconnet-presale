using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Extensions.Repository;

public static class UserAccountExtensions
{
    public static async Task<UserAccount?> GetUserAccountByIdAsync(this IAppDbContext context, Guid userAccountId)
    {
        return await context.UserAccounts
            .Include(x => x.User)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.UserAccountId == userAccountId);
    }

    public static async Task<UserAccount?> GetUserAccountByUsernameAsync(this IAppDbContext context, string username)
    {
        return await context.UserAccounts
            .Include(x => x.User)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.User.Username == username);
    }

    public static async Task<UserAccount?> GetUserAccountByEmailAddressAsync(this IAppDbContext context, string emailAddress)
    {
        return await context.UserAccounts
            .Include(x => x.User)
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(x => x.User.EmailAddress == emailAddress);
    }

    public static async Task<List<UserAccount>> GetUserAccountAsync(this IAppDbContext context)
    {
        return await context.UserAccounts
            .Include(x => x.User)
            .Include(x => x.UserProfile)
            .ToListAsync();
    }
}
