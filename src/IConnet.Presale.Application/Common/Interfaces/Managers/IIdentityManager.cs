using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IIdentityManager
{
    Task<Result> TryEditUserAccount(Guid userAccountId, string newUsername,
        string newPassword, bool isChangeUsername, bool isChangePassword);
    Task<Result<UserAccount>> TrySignUpAsync(string username, string password,
        string statusEmploymentString, string userRoleString, string jobTitle,
        bool autoPrivilege = false);
    Task<Result> TrySetRoleAsync(Guid userAccountId, string role);
    Task<Result> TryGrantPrivilegeAsync(Guid userAccountId, string privilege);
    Task<Result> TryRevokePrivilegeAsync(Guid userAccountId, string privilege);
    Task<Result> TryResetPasswordAsync(Guid userAccountId, string oldPassword, string newPassword);
}
