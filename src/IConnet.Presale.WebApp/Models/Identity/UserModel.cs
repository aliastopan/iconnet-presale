using System.Security.Claims;
using IConnet.Presale.Infrastructure.Security;

namespace IConnet.Presale.WebApp.Models.Identity;

public sealed class UserModel
{
    public UserModel(ClaimsPrincipal principal)
    {
        Username = principal.FindFirst(c => c.Type == JwtClaimTypes.UniqueName)!.Value;
        Role = principal.FindFirst(c => c.Type == JwtClaimTypes.Role)!.Value;
        Privileges = principal.FindAll(c => c.Type == JwtClaimTypes.Privileges)
            .Select(c => c.Value)
            .ToList();
        JobTitle = principal.FindFirst(c => c.Type == JwtClaimTypes.JobTitle)!.Value;
    }

    public string Username { get; set; }
    public string Role { get; set; }
    public ICollection<string> Privileges { get; set; }
    public string JobTitle { get; set; }
}