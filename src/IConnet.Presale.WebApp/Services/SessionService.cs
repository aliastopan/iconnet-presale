using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.WebApp.Services;

public sealed class SessionService
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly NavigationManager _navigationManager;

    public SessionService(ProtectedLocalStorage localStorage,
        NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    public UserModel? UserModel { get; private set; }
    public bool HasUser => UserModel is not null;

    public void SetSession(ClaimsPrincipal principal)
    {
        UserModel = new UserModel(principal);
    }

    public async Task SignOutAsync()
    {
        await _localStorage.DeleteAsync("access-token");
        await _localStorage.DeleteAsync("refresh-token");

        _navigationManager.NavigateTo("/", forceLoad: true);
    }

    public string GetSessionAlias()
    {
        return UserModel switch
        {
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {GetRoleString("PH")}",
            { Role: UserRole.PlanningAssetCoverage } => $"{UserModel!.Username} {GetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {GetRoleString("ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    public async Task<string> GetSessionAliasAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return UserModel switch
        {
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {GetRoleString("PH")}",
            { Role: UserRole.PlanningAssetCoverage } => $"{UserModel!.Username} {GetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {GetRoleString("ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    public async Task<string> GetJobShiftAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return UserModel!.JobShift.ToString();
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
