using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Persistence;

public interface IAppDbContext : IDisposable
{
    // aggregates
    DbSet<UserAccount> UserAccounts { get; }

    // entities
    DbSet<RefreshToken> RefreshTokens { get; }


    int SaveChanges();
    Task<int> SaveChangesAsync();
}
