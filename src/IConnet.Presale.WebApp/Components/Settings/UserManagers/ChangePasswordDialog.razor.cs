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

    protected PasswordChangeModel PasswordChange = new PasswordChangeModel();

    protected List<Error> Errors => _errors;
    protected bool ShowErrorMessages => _errors.Count > 0;

    protected async Task SaveAsync()
    {
        _errors.Clear();

        var isValid = PasswordChange.TryValidate(out var errors);

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
        throw new NotImplementedException();
    }

    protected void OnNewPasswordChanged(string newPassword)
    {
        PasswordChange.NewPassword = newPassword;
    }

    protected void OnConfirmPasswordChanged(string confirmPassword)
    {
        PasswordChange.ConfirmPassword = confirmPassword;
    }
}

public class PasswordChangeModel
{
    public PasswordChangeModel()
    {

    }

    public PasswordChangeModel(string newPassword, string confirmPassword)
    {
        NewPassword = newPassword;
        ConfirmPassword = confirmPassword;
    }

    [Required(ErrorMessage = "Password tidak boleh kosong.")]
    [RegularExpression(RegexPattern.StrongPassword, ErrorMessage = "Password harus mengandung setidaknya satu digit, satu huruf kecil, satu huruf besar, dan setidaknya terdiri dari 8 karakter.")]
    public string NewPassword { get; set; } = default!;

    [Required(ErrorMessage = "Konfirmasi password tidak boleh kosong.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Password tidak sesuai.")]
    public string ConfirmPassword { get; set; } = default!;
}
