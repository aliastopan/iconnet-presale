#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Identity;

public class User
{
    public Guid UserId { get; init; }
    public string Username { get; init; }
    public string EmailAddress { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public UserRole UserRole { get; set; }
    public ICollection<UserPrivilege> UserPrivileges { get; set; }
    public string JobTitle { get; set; }
    public JobShift JobShift { get; set; }
}
