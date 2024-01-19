using Microsoft.Extensions.Configuration;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;

[assembly: InternalsVisibleTo("IConnet.Presale.Tests")]
namespace IConnet.Presale.Infrastructure.Persistence;

internal sealed class AppDbContextSeeder : IAppDbContextSeeder
{
    private readonly IAppDbContextFactory<IAppDbContext> _dbContextFactory;
    private readonly IPasswordService _passwordService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;

    public AppDbContextSeeder(IAppDbContextFactory<IAppDbContext> dbContextFactory,
        IPasswordService passwordService,
        IDateTimeService dateTimeService,
        IConfiguration configuration)
    {
        _dbContextFactory = dbContextFactory;
        _passwordService = passwordService;
        _dateTimeService = dateTimeService;
        _configuration = configuration;
    }

    public async Task<int> GenerateUsersAsync()
    {
        var userAccount01 = new UserAccount
        {
            UserAccountId = Guid.Parse("9dd0aa01-3a6e-4159-8c7b-8ee4caa1d4ea"),
            User = new User
            {
                Username = "erasmus",
                EmailAddress = "erasmus@proton.me",
                UserRole = UserRole.Administrator,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                    UserPrivilege.Administrator
                }
            },
            UserProfile = new UserProfile
            {
                FirstName = "Taufan",
                LastName = "Augusta",
                DateOfBirth = new DateOnly(year: 1996, month: 8, day: 19)
            },
            PasswordHash = _passwordService.HashPassword(_configuration["Credentials:Administrator"]!, out var salt),
            PasswordSalt = salt,
            IsVerified = true,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.UserAccounts.Add(userAccount01);
        return await dbContext.SaveChangesAsync();
    }
}
