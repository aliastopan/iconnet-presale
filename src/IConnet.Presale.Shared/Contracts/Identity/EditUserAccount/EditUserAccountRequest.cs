namespace IConnet.Presale.Shared.Contracts.Identity.EditUserAccount;

public record EditUserAccountRequest : IEditUserAccountModel
{
    public EditUserAccountRequest(Guid userAccountId, string newUsername,
        string newPassword, string confirmPassword,
        bool isChangeUsername, bool isChangePassword)
    {
        UserAccountId = userAccountId;
        NewUsername = newUsername;
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;

        IsChangeUsername = isChangeUsername;
        IsChangePassword = isChangePassword;
    }

    public Guid UserAccountId { get; }
    public string NewUsername { get; }
    public string NewPassword { get; }
    public string ConfirmPassword { get; }

    public bool IsChangeUsername { get; }
    public bool IsChangePassword { get; }
}
