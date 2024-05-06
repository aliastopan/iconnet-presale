using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Identity;

namespace IConnet.Presale.Application.Identity.Commands.EditUserAccount;

public class EditUserAccountCommand(Guid userAccountId, string newUsername,
    string newPassword, string confirmPassword, bool isChangeUsername, bool isChangePassword)
    : IEditUserAccountModel, IRequest<Result>
{
    [Required]
    public Guid UserAccountId { get; init; } = userAccountId;

    [Required]
    [RegularExpression(RegexPattern.Username)]
    public string NewUsername { get; init; } = newUsername;

    [Required]
    [RegularExpression(RegexPattern.StrongPassword)]
    public string NewPassword { get; init; } = newPassword;

    [Required]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match.")]
    public string ConfirmPassword { get; init; } = confirmPassword;

    public bool IsChangeUsername { get; init; } = isChangeUsername;
    public bool IsChangePassword { get; init; } = isChangePassword;
}
