namespace IConnet.Presale.Shared.Interfaces.Models.Identity;

public interface IEditUserAccountModel
{
    Guid UserAccountId { get; }
    string NewUsername { get; }
    string NewPassword { get; }
    string ConfirmPassword { get; }

    bool IsChangeUsername { get; }
    bool IsChangePassword { get; }
}
