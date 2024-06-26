using System.Reflection;
using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Entities;
using System.Dynamic;
using IConnet.Presale.Domain.Aggregates.Presales;

[assembly: InternalsVisibleTo("IConnet.Presale.Tests")]
namespace IConnet.Presale.Infrastructure.Persistence;

internal sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    // aggregates
    public DbSet<WorkPaper> WorkPapers => Set<WorkPaper>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    // entities
    public DbSet<ApprovalOpportunity> ApprovalOpportunities => Set<ApprovalOpportunity>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ChatTemplate> ChatTemplates => Set<ChatTemplate>();
    public DbSet<DirectApproval> DirectApprovals => Set<DirectApproval>();
    public DbSet<RepresentativeOffice> RepresentativeOffices => Set<RepresentativeOffice>();
    public DbSet<RootCause> RootCauses => Set<RootCause>();

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
