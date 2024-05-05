namespace IConnet.Presale.Shared.Contracts.Identity.EditUserAccount;

public record EditUserAccountRequest : IEditUserAccountModel
{
    public EditUserAccountRequest(Guid userAccountId, string username,
        string newPassword, string confirmPassword,
        bool isChangeUsername, bool isChangePassword)
    {
        UserAccountId = userAccountId;
        Username = username;
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;

        IsChangeUsername = isChangeUsername;
        IsChangePassword = isChangePassword;
    }

    public Guid UserAccountId { get; }
    public string Username { get; }
    public string NewPassword { get; }
    public string ConfirmPassword { get; }

    public bool IsChangeUsername { get; }
    public bool IsChangePassword { get; }
}
