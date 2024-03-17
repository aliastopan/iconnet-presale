using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.WebApp.Services;

public sealed class SessionService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly NavigationManager _navigationManager;
    private readonly OptionService _optionService;
    private readonly FilterPreference _filterPreference;

    public SessionService(IDateTimeService dateTimeService,
        ProtectedLocalStorage localStorage,
        NavigationManager navigationManager,
        OptionService optionService)
    {
        _dateTimeService = dateTimeService;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
        _optionService = optionService;
        _filterPreference = new FilterPreference(optionService.KantorPerwakilanOptions);
    }

    public UserModel? UserModel { get; private set; }
    public bool HasUser => UserModel is not null;
    public FilterPreference FilterPreference => _filterPreference;

    public async Task<ActionSignature> GenerateActionSignatureAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return new ActionSignature
        {
            AccountIdSignature = UserModel!.UserAccountId,
            Alias = GetSessionAlias(),
            TglAksi = _dateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    public async Task SetSessionAsync(ClaimsPrincipal principal)
    {
        try
        {
            UserModel = new UserModel(principal);
        }
        catch (Exception exception)
        {
            Log.Warning("Unable to set session", exception.Message);

            await _localStorage.DeleteAsync("access-token");
            await _localStorage.DeleteAsync("refresh-token");
        }
    }

    public async Task SignOutAsync()
    {
        await _localStorage.DeleteAsync("access-token");
        await _localStorage.DeleteAsync("refresh-token");

        _navigationManager.NavigateTo("/", forceLoad: true);
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

    public string GetSessionAlias()
    {
        return UserModel switch
        {
            { Role: UserRole.Helpdesk } => $"{UserModel!.Username} {SetRoleString("PH")}",
            { Role: UserRole.PAC } => $"{UserModel!.Username} {SetRoleString("PAC")}",
            { Role: UserRole.Administrator } => $"{UserModel!.Username} {SetRoleString("ADMIN")}",
            { Role: UserRole.SuperUser } => $"{UserModel!.Username} {SetRoleString("SUPER ADMIN")}",
            _ => $"{UserModel!.Username} (GUEST)"
        };
    }

    public async Task<string> GetSessionAliasAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return GetSessionAlias();
    }

    public async Task<Guid> GetUserAccountIdAsync()
    {
        if (UserModel is null)
        {
            await SignOutAsync();
        }

        return UserModel!.UserAccountId;
    }

    public string GetShift()
    {
        var now = _dateTimeService.DateTimeOffsetNow;
        int currentHour = now.Hour;

        if (currentHour >= 8 && currentHour < 14)
        {
            return EnumProcessor.EnumToDisplayString(JobShift.Siang);
        }
        else if (currentHour >= 14 && currentHour < 21)
        {
            return EnumProcessor.EnumToDisplayString(JobShift.Malam);
        }
        else
        {
            return EnumProcessor.EnumToDisplayString(JobShift.OverShift);
        }
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
