using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Shared.Contracts.Identity.ResetPassword;

public record ResetPasswordRequest(Guid UserAccountId, string OldPassword, string NewPassword, string ConfirmPassword)
    : IResetPasswordModel;