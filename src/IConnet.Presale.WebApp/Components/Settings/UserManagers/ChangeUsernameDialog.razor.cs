using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using IConnet.Presale.WebApp.Models.Identity;
using IConnet.Presale.Shared.Validations;

namespace IConnet.Presale.WebApp.Components.Settings.UserManagers;

public partial class ChangeUsernameDialog : IDialogContentComponent<EditUserAccountModel>
{
    [Parameter]
    public EditUserAccountModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private readonly List<Error> _errors = [];

    protected List<Error> Errors => _errors;
    protected bool ShowErrorMessages => _errors.Count > 0;

    protected UsernameChangeModel UsernameChange = new UsernameChangeModel();

    protected async Task SaveAsync()
    {
        _errors.Clear();

        var isValid = UsernameChange.TryValidate(out var errors);

        if (!isValid)
        {
            _errors.AddRange(errors);

            return;
        }

        var editResult = new  EditUserAccountModel(Content.UserAccountId)
        {
            NewUsername = UsernameChange.NewUsername,
            IsChangeUsername = true
        };

        await Dialog.CloseAsync(editResult);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnNewUsernameChanged(string newUsername)
    {
        UsernameChange.NewUsername = newUsername;
    }
}

public class UsernameChangeModel
{
    public UsernameChangeModel()
    {

    }

    public UsernameChangeModel(string newUsername)
    {
        NewUsername = newUsername;
    }

    [Required(ErrorMessage = "Username tidak boleh kosong.")]
    [RegularExpression(RegexPattern.Username, ErrorMessage = "Username harus terdiri dari 3-16 karakter, jangan awali atau akhiri dengan garis bawah/titik, boleh alfanumerik, garis bawah, titik, tidak boleh ada spasi.")]
    public string NewUsername { get; set; } = default!;
}
