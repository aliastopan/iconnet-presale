using System.Security.Claims;
using IConnet.Presale.WebApp.Models.Identity;

using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.WebApp.Services;

public sealed class SessionService
{
    public UserModel? UserModel { get; private set; }
    public bool HasUser => UserModel is not null;
    public string SessionAlias => GetSessionAlias();

    public void SetSession(ClaimsPrincipal principal)
    {
        UserModel = new UserModel(principal);
    }

    public void UnsetSession()
    {
        UserModel = null;
    }

    private string GetSessionAlias()
    {
        return UserModel switch
        {
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {GetRoleString("PH")}",
            { Role: UserRole.PlanningAssetCoverage } => $"{UserModel!.Username} {GetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {GetRoleString("ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    private string GetRoleString(string role)
    {
        if (UserModel!.EmploymentStatus == EmploymentStatus.Intern)
        {
            return $"({role}/MAGANG)";
        }

        return $"({role})";
    }

}
