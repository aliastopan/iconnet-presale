using System.Security.Claims;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Infrastructure.Security;

namespace IConnet.Presale.WebApp.Models.Identity;

public sealed class UserModel
{
    public UserModel(ClaimsPrincipal principal)
    {
        var jwtSub = principal.FindFirst(c => c.Type == JwtClaimTypes.Sub)!.Value;
        UserAccountId = Guid.Parse(jwtSub);

        Username = principal.FindFirst(c => c.Type == JwtClaimTypes.UniqueName)!.Value;

        var employmentStatus = principal.FindFirst(c => c.Type == JwtClaimTypes.EmploymentStatus)!.Value;
        EmploymentStatus = (EmploymentStatus)Enum.Parse(typeof(EmploymentStatus), employmentStatus);

        var userRole = principal.FindFirst(c => c.Type == JwtClaimTypes.Role)!.Value;
        Role = (UserRole)Enum.Parse(typeof(UserRole), userRole);

        var privileges = principal.FindAll(c => c.Type == JwtClaimTypes.Privileges)
            .Select(c => c.Value)
            .ToList();

        foreach (var userPrivilege in privileges)
        {
            var privilege = (UserPrivilege)Enum.Parse(typeof(UserPrivilege), userPrivilege);
            Privileges.Add(privilege);
        }

        JobTitle = principal.FindFirst(c => c.Type == JwtClaimTypes.JobTitle)!.Value;

        var jobShift = principal.FindFirst(c => c.Type == JwtClaimTypes.JobShift)!.Value;
        JobShift = (JobShift)Enum.Parse(typeof(JobShift), jobShift);
    }

    public Guid UserAccountId { get; set; }
    public string Username { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public UserRole Role { get; set; }
    public ICollection<UserPrivilege> Privileges { get; set; } = [];
    public string JobTitle { get; set; }
    public JobShift JobShift { get; set; }
}

