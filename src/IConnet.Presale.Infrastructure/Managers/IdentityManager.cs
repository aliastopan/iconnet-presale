using IConnet.Presale.Domain.Aggregates.Identity;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class IdentityManager : IIdentityManager
{
    private readonly IIdentityAggregateHandler _identityAggregateHandler;

    public IdentityManager(IIdentityAggregateHandler identityAggregateHandler)
    {
        _identityAggregateHandler = identityAggregateHandler;
    }

    public async Task<Result> TryEditUserAccount(Guid userAccountId, string newUsername,
        string newPassword, bool isChangeUsername, bool isChangePassword)
    {
        if (!isChangeUsername && !isChangePassword)
        {
            return Result.Ok();
        }

        var tryGetUserAccount = await _identityAggregateHandler.TryGetUserAccountAsync(userAccountId);

        if (tryGetUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryGetUserAccount);
        }

        var userAccount = tryGetUserAccount.Value;

        await _identityAggregateHandler.EditUserAccount(userAccount, newUsername, newPassword, isChangeUsername, isChangePassword );
        await _identityAggregateHandler.InvalidateRefreshTokensAsync(userAccount);

        return Result.Ok();
    }

    public async Task<Result<UserAccount>> TrySignUpAsync(string username, string password,
        string statusEmploymentString, string userRoleString, string jobTitle,
        bool autoPrivilege = false)
    {
        var TryValidateAvailability = await _identityAggregateHandler.TryValidateAvailabilityAsync(username);
        if (TryValidateAvailability.IsFailure())
        {
            return Result<UserAccount>.Inherit(result: TryValidateAvailability);
        }

        EmploymentStatus employmentStatus;
        UserRole userRole;

        try
        {
            employmentStatus = (EmploymentStatus)Enum.Parse(typeof(EmploymentStatus), statusEmploymentString);
            userRole = (UserRole)Enum.Parse(typeof(UserRole), userRoleString);
        }
        catch (Exception exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Error);
            return Result<UserAccount>.Error(error);
        }

        var userAccount = await _identityAggregateHandler.CreateUserAccountAsync(username, password,
            employmentStatus, userRole, jobTitle,
            autoPrivilege);

        return Result<UserAccount>.Ok(userAccount);
    }

    public async Task<Result> TrySetRoleAsync(Guid userAccountId, string role)
    {
        var tryGetUserAccount = await _identityAggregateHandler.TryGetUserAccountAsync(userAccountId);
        if (tryGetUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryGetUserAccount);
        }

        var userAccount = tryGetUserAccount.Value;
        var userRole = (UserRole)Enum.Parse(typeof(UserRole), role);

        await _identityAggregateHandler.UpdateUserRoleAsync(userAccount, userRole);

        return Result.Ok();
    }

    public async Task<Result> TryGrantPrivilegeAsync(Guid userAccountId, string privilege)
    {
        var tryGetUserAccount = await _identityAggregateHandler.TryGetUserAccountAsync(userAccountId);
        if (tryGetUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryGetUserAccount);
        }

        UserPrivilege userPrivilege;

        try
        {
            userPrivilege = (UserPrivilege)Enum.Parse(typeof(UserPrivilege), privilege);
        }
        catch (Exception exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Error);
            return Result<UserAccount>.Error(error);
        }

        var userAccount = tryGetUserAccount.Value;
        var hasDuplicatePrivilege = userAccount.User.UserPrivileges.Contains(userPrivilege);

        if (hasDuplicatePrivilege)
        {
            var error = new Error("Cannot have duplicate privilege.", ErrorSeverity.Warning);
            return Result.Conflict(error);
        }

        await _identityAggregateHandler.GrantPrivilegeAsync(userAccount, userPrivilege);

        return Result.Ok();
    }

    public async Task<Result> TryRevokePrivilegeAsync(Guid userAccountId, string privilege)
    {
        var tryGetUserAccount = await _identityAggregateHandler.TryGetUserAccountAsync(userAccountId);
        if (tryGetUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryGetUserAccount);
        }

        UserPrivilege userPrivilege;

        try
        {
            userPrivilege = (UserPrivilege)Enum.Parse(typeof(UserPrivilege), privilege);
        }
        catch (Exception exception)
        {
            var error = new Error(exception.Message, ErrorSeverity.Error);
            return Result<UserAccount>.Error(error);
        }

        var userAccount = tryGetUserAccount.Value;
        var hasMissingPrivilege = !userAccount.User.UserPrivileges.Contains(userPrivilege);
        if (hasMissingPrivilege)
        {
            var error = new Error("Privilege does not exist.", ErrorSeverity.Warning);
            return Result.Invalid(error);
        }

        await _identityAggregateHandler.RevokePrivilegeAsync(userAccount, userPrivilege);;

        return Result.Ok();
    }

    public async Task<Result> TryResetPasswordAsync(Guid userAccountId, string oldPassword, string newPassword)
    {
        var tryGetUserAccount = await _identityAggregateHandler.TryGetUserAccountAsync(userAccountId);
        if (tryGetUserAccount.IsFailure())
        {
            return Result.Inherit(result: tryGetUserAccount);
        }

        var userAccount = tryGetUserAccount.Value;
        var tryValidatePassword = _identityAggregateHandler.TryValidatePassword(newPassword, oldPassword, userAccount.PasswordSalt, userAccount.PasswordHash);
        if (tryValidatePassword.IsFailure())
        {
            return Result.Inherit(result: tryValidatePassword);
        }

        await _identityAggregateHandler.UpdatePasswordAsync(userAccount, newPassword);
        await _identityAggregateHandler.InvalidateRefreshTokensAsync(userAccount);

        return Result.Ok();
    }
}
