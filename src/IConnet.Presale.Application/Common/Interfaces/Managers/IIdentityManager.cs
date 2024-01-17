using IConnet.Presale.Domain.Aggregates.Identity;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IIdentityManager
{
    Task<Result<UserAccount>> TrySignUpAsync(string username, string firstName, string lastName,
        DateOnly dateOfBirth, string emailAddress, string password);
    Task<Result> TrySetRoleAsync(Guid userAccountId, string role);
    Task<Result> TryGrantPrivilegeAsync(Guid userAccountId, string privilege);
    Task<Result> TryRevokePrivilegeAsync(Guid userAccountId, string privilege);
    Task<Result> TryResetPasswordAsync(Guid userAccountId, string oldPassword, string newPassword);
}
