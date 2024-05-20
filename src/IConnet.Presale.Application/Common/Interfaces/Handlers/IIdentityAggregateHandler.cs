using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Application.Common.Interfaces.Handlers;

public interface IIdentityAggregateHandler
{
    // user account
    Task EditUserAccount(UserAccount userAccount, string newUsername,
        string newPassword, bool isChangeUsername, bool isChangePassword);
    Task<UserAccount> CreateUserAccountAsync(string username, string password,
        EmploymentStatus employmentStatus, UserRole userRole, string jobTitle,
        bool autoPrivilege = false);
    Task SignUserAsync(UserAccount userAccount, RefreshToken refreshToken);
    Task<Result<UserAccount>> TryGetUserAccountAsync(Guid userAccountId);
    Task<Result<UserAccount>> TryGetUserAccountAsync(string username);
    Task<Result<List<UserAccount>>> TryGetRangeUserAccountsAsync(int range = 0);
    Task<Result> TryValidateAvailabilityAsync(string username);

    // user role
    Task UpdateUserRoleAsync(UserAccount userAccount, UserRole userRole);

    // user privilege
    Task GrantPrivilegeAsync(UserAccount userAccount, UserPrivilege userPrivilege);
    Task RevokePrivilegeAsync(UserAccount userAccount, UserPrivilege userPrivilege);

    // password
    Result TryValidatePassword(string password, string passwordSalt, string passwordHash);
    Result TryValidatePassword(string newPassword, string oldPassword, string passwordSalt, string passwordHash);
    Task UpdatePasswordAsync(UserAccount userAccount, string newPassword);

    // refresh token
    Task InvalidateRefreshTokensAsync(UserAccount userAccount);
    Task RotateRefreshTokenAsync(RefreshToken previous, RefreshToken current);

    // presale specified
    Task<Result<List<UserAccount>>> TryGetRangePresaleOperatorAsync();
}
