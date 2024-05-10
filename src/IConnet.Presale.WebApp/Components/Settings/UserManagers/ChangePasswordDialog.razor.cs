using Microsoft.AspNetCore.Components.Web;
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

    protected bool ShowPassword = false;
    protected Icon PasswordIcon => ShowPassword
        ? new Icons.Filled.Size20.Eye().WithColor("var(--info-grey)")
        : new Icons.Filled.Size20.EyeOff().WithColor("#bdbbbb");
    protected TextFieldType PasswordTextFieldType => ShowPassword
        ? TextFieldType.Text
        : TextFieldType.Password;

    protected bool ShowConfirmationPassword = false;
    protected Icon ConfirmationPasswordIcon => ShowConfirmationPassword
        ? new Icons.Filled.Size20.Eye().WithColor("var(--info-grey)")
        : new Icons.Filled.Size20.EyeOff().WithColor("#bdbbbb");
    protected TextFieldType ConfirmationPasswordTextFieldType => ShowConfirmationPassword
        ? TextFieldType.Text
        : TextFieldType.Password;

    protected async Task SaveAsync()
    {
        _errors.Clear();

        var isValid = PasswordChange.TryValidate(out var errors);

        if (!isValid)
        {
            _errors.AddRange(errors);

            return;
        }

        var editResult = new  EditUserAccountModel(Content.UserAccountId)
        {
            NewPassword = PasswordChange.NewPassword,
            ConfirmPassword = PasswordChange.ConfirmPassword,
            IsChangePassword = true
        };

        await Dialog.CloseAsync(editResult);
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

    protected void OnToggleShowPassword(MouseEventArgs args)
    {
        ShowPassword = !ShowPassword;
    }

    protected void OnToggleShowConfirmationPassword(MouseEventArgs args)
    {
        ShowConfirmationPassword = !ShowConfirmationPassword;
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
