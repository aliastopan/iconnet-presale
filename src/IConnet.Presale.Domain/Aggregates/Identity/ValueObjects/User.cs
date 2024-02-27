#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Identity.ValueObjects;

public class User : ValueObject
{
    public string Username { get; init; }
    public string EmailAddress { get; init; }
    public EmploymentStatus EmploymentStatus { get; init; }
    public UserRole UserRole { get; init; }
    public IReadOnlyCollection<UserPrivilege> UserPrivileges { get; init; }
    public string JobTitle { get; init; }
    public JobShift JobShift { get; init; }

    public User ChangeEmailAddress(string emailAddress)
    {
        return new User
        {
            Username = this.Username,
            EmailAddress = emailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = this.UserRole,
            UserPrivileges = this.UserPrivileges,
            JobTitle = this.JobTitle,
            JobShift = this.JobShift
        };
    }

    public User ChangeUserRole(UserRole userRole)
    {
        return new User
        {
            Username = this.Username,
            EmailAddress = this.EmailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = userRole,
            UserPrivileges = this.UserPrivileges,
            JobTitle = this.JobTitle,
            JobShift = this.JobShift
        };
    }

    public User AddPrivilege(UserPrivilege privilege)
    {
        var privileges = new List<UserPrivilege>(this.UserPrivileges)
        {
            privilege
        };

        return new User
        {
            Username = this.Username,
            EmailAddress = this.EmailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = this.UserRole,
            UserPrivileges = privileges,
            JobTitle = this.JobTitle,
            JobShift = this.JobShift
        };
    }

    public User RemovePrivilege(UserPrivilege privilege)
    {
        var privileges = new List<UserPrivilege>(this.UserPrivileges);
        privileges.Remove(privilege);

        return new User
        {
            Username = this.Username,
            EmailAddress = this.EmailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = this.UserRole,
            UserPrivileges = privileges,
            JobTitle = this.JobTitle,
            JobShift = this.JobShift
        };
    }

    public User ChangeJobTitle(string jobTitle)
    {
        return new User
        {
            Username = this.Username,
            EmailAddress = this.EmailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = this.UserRole,
            UserPrivileges = this.UserPrivileges,
            JobTitle = jobTitle,
            JobShift = this.JobShift
        };
    }

    public User ChangeJobShift(JobShift jobShift)
    {
        return new User
        {
            Username = this.Username,
            EmailAddress = this.EmailAddress,
            EmploymentStatus = this.EmploymentStatus,
            UserRole = this.UserRole,
            UserPrivileges = this.UserPrivileges,
            JobTitle = this.JobTitle,
            JobShift = jobShift
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return EmailAddress;
        yield return EmploymentStatus;
        yield return UserRole;
        yield return UserPrivileges;
        yield return JobTitle;
        yield return JobShift;
    }
}