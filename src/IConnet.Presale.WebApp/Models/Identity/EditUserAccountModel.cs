using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.WebApp.Models.Identity;

public class EditUserAccountModel : IEditUserAccountModel
{
    public EditUserAccountModel(Guid userAccountId)
    {
        UserAccountId = userAccountId;
    }

    public Guid UserAccountId { get; init; }
    public string CurrentUsername { get; set; } = default!;
    public string NewUsername { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public bool IsChangeUsername { get; set; }
    public bool IsChangePassword { get; set; }
}
