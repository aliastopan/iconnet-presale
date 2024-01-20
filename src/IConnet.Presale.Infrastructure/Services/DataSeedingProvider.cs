using Microsoft.Extensions.Configuration;
using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;

[assembly: InternalsVisibleTo("IConnet.Presale.Tests")]
namespace IConnet.Presale.Infrastructure.Services;

internal sealed class DataSeedingProvider : IDataSeedingService
{
    private readonly AppDbContextFactory _dbContextFactory;
    private readonly IPasswordService _passwordService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IConfiguration _configuration;

    public DataSeedingProvider(AppDbContextFactory dbContextFactory,
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
        var administrator01 = new UserAccount
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
                },
                UserShift = UserShift.Siang
            },
            UserProfile = new UserProfile
            {
                FirstName = "Taufan",
                LastName = "Augusta",
                DateOfBirth = new DateOnly(year: 1996, month: 8, day: 19)
            },
            PasswordHash = _passwordService.HashPassword(_configuration["Credentials:Administrator"]!, out var salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        var helpdesk01 = new UserAccount
        {
            UserAccountId = Guid.Parse("73946fa9-d92d-453a-a5c4-1f3daea5d66f"),
            User = new User
            {
                Username = "helpdesk",
                EmailAddress = "helpdesk.first@mail.me",
                UserRole = UserRole.Helpdesk,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                UserShift = UserShift.Siang
            },
            UserProfile = new UserProfile
            {
                FirstName = "Andy",
                LastName = "Wijaya",
                DateOfBirth = new DateOnly(year: 1997, month: 5, day: 10)
            },
            PasswordHash = _passwordService.HashPassword("pwd", out salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        var pac01 = new UserAccount
        {
            UserAccountId = Guid.Parse("3eadb218-b95c-48e9-886c-7859de74ba76"),
            User = new User
            {
                Username = "pac",
                EmailAddress = "pac.first@mail.me",
                UserRole = UserRole.PlanningAssetCoverage,
                UserPrivileges = new List<UserPrivilege>()
                {
                    UserPrivilege.Viewer,
                    UserPrivilege.Editor,
                },
                UserShift = UserShift.Siang
            },
            UserProfile = new UserProfile
            {
                FirstName = "Rizky",
                LastName = "Moneter",
                DateOfBirth = new DateOnly(year: 1998, month: 11, day: 7)
            },
            PasswordHash = _passwordService.HashPassword("pwd", out salt),
            PasswordSalt = salt,
            CreationDate = _dateTimeService.DateTimeOffsetNow,
            LastSignedIn = _dateTimeService.DateTimeOffsetNow
        };

        using var dbContext = _dbContextFactory.CreateDbContext();

        dbContext.UserAccounts.Add(administrator01);
        dbContext.UserAccounts.Add(helpdesk01);
        dbContext.UserAccounts.Add(pac01);

        return await dbContext.SaveChangesAsync();
    }
}
