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
    private readonly OptionService _optionService;
    private readonly FilterPreference _filterPreference;

    public SessionService(ProtectedLocalStorage localStorage,
        NavigationManager navigationManager,
        OptionService optionService)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
        _optionService = optionService;
        _filterPreference = new FilterPreference(optionService.KantorPerwakilanOptions);
    }

    public UserModel? UserModel { get; private set; }
    public bool HasUser => UserModel is not null;
    public FilterPreference FilterPreference => _filterPreference;

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
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {SetRoleString("PH")}",
            { Role: UserRole.PlanningAssetCoverage } => $"{UserModel!.Username} {SetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {SetRoleString("ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    public async Task<bool> IsAliasMatch(string alias)
    {
        var sessionAlias = await GetSessionAliasAsync();
        if (sessionAlias != alias)
        {
            return false;
        }

        return true;
    }

    public async Task<string> GetSessionAliasAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return UserModel switch
        {
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {SetRoleString("PH")}",
            { Role: UserRole.PlanningAssetCoverage } => $"{UserModel!.Username} {SetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {SetRoleString("ADMIN")}",
            { Role: UserRole.SuperUser } => $"{UserModel!.Username} {SetRoleString("SUPER ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    public async Task<Guid> GetUserAccountIdAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return UserModel!.UserAccountId;
    }

    private string SetRoleString(string role)
    {
        if (UserModel!.EmploymentStatus == EmploymentStatus.Magang)
        {
            return $"({role}/MAGANG)";
        }

        return $"({role})";
    }
}
