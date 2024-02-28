#nullable disable
using IConnet.Presale.Domain.Aggregates.Identity.ValueObjects;

namespace IConnet.Presale.Domain.Aggregates.Identity;

public class UserAccount : IAggregateRoot
{
    public UserAccount()
    {

    }

    public UserAccount(string username, string emailAddress, string passwordHash, string passwordSalt,
        EmploymentStatus employmentStatus, UserRole userRole, string jobTitle, JobShift jobShift,
        DateTimeOffset creationDate)
    {
        User = new User
        {
            Username = username,
            EmailAddress = emailAddress,
            EmploymentStatus = employmentStatus,
            UserRole = userRole,
            UserPrivileges = new List<UserPrivilege>()
            {
                UserPrivilege.Viewer
            },
            JobTitle = jobTitle,
            JobShift = jobShift
        };

        UserAccountId = Guid.NewGuid();
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        CreationDate = creationDate;
        LastSignedIn = creationDate;
    }

    public Guid UserAccountId { get; init; }
    public User User { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastSignedIn { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; init; }
}
