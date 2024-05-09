using System.ComponentModel.DataAnnotations;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Validations;

namespace IConnet.Presale.WebApp.Components.Settings.UserManagers;

public partial class ChangePasswordDialog : IDialogContentComponent<EditUserAccountModel>
{
    [Parameter]
    public EditUserAccountModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private readonly List<Error> _errors = [];

    protected string NewPassword { get; set; } = default!;
    protected string ConfirmPassword { get; set; } = default!;

    protected List<Error> Errors => _errors;
    protected bool ShowErrorMessages => _errors.Count > 0;

    protected async Task SaveAsync()
    {
        _errors.Clear();

        var passwordCheck = new PasswordCheckModel(NewPassword, ConfirmPassword);
        var isValid = passwordCheck.TryValidate(out var errors);

        if (!isValid)
        {
            _errors.AddRange(errors);

            return;
        }


        await Dialog.CloseAsync(new EditUserAccountModel(Content.UserAccountId));
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected bool ChangePasswordAsync()
    {
        var passwordCheck = new PasswordCheckModel(NewPassword, ConfirmPassword);


        throw new NotImplementedException();
    }

    protected void OnNewPasswordChanged(string newPassword)
    {
        NewPassword = newPassword;
    }

    protected void OnConfirmPasswordChanged(string confirmPassword)
    {
        ConfirmPassword = confirmPassword;
    }
}

public class PasswordCheckModel
{
    public PasswordCheckModel(string password, string confirmPassword)
    {
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    [Required(ErrorMessage = "Password tidak boleh kosong.")]
    [RegularExpression(RegexPattern.StrongPassword, ErrorMessage = "Password harus mengandung setidaknya satu digit, satu huruf kecil, satu huruf besar, dan setidaknya terdiri dari 8 karakter.")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Konfirmasi password tidak boleh kosong.")]
    [Compare(nameof(Password), ErrorMessage = "Password tidak sesuai.")]
    public string ConfirmPassword { get; set; } = default!;
}
