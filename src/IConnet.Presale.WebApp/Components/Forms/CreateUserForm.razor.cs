using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;
using Microsoft.AspNetCore.Components.Web;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class CreateUserForm : ComponentBase
{
    [Inject] public IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private readonly List<Error> _errors = [];

    protected CreateUserModel CreateUserModel { get; set; } = default!;
    protected List<Error> Errors => _errors;
    protected bool IsLoading { get; set; } = false;
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

    protected Func<string, bool> OptionDisableSuperUser => option => option == OptionSelect.Role.SuperUser;

    protected override void OnInitialized()
    {
        CreateUserModel = new CreateUserModel
        {
            UserRole = OptionSelect.Role.RoleOptions.First(),
            EmploymentStatus = OptionSelect.StatusKepegawaian.StatusKepegawaianOptions.First(),
            IsManagedByAdministrator = true
        };

        base.OnInitialized();
    }

    protected async Task SubmitAsync()
    {
        _errors.Clear();

        var isValid = CreateUserModel.TryValidate(out Error[] errors);

        if (!isValid)
        {
            // LogSwitch.Debug("Errors {0}", _errors.Count);
            _errors.AddRange(errors);

            return;
        }

        await CreateUserAsync();
    }

    protected async Task CreateUserAsync()
    {
        IsLoading = true;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await IdentityHttpClient.SignUpAsync(
            CreateUserModel.Username,
            CreateUserModel.Password,
            EnumProcessor.DisplayStringToEnumString(CreateUserModel.EmploymentStatus),
            EnumProcessor.DisplayStringToEnumString(CreateUserModel.UserRole),
            CreateUserModel.JobTitle,
            CreateUserModel.IsManagedByAdministrator);

        if (httpResult.IsSuccessStatusCode)
        {
            var intent = ToastIntent.Success;
            var message = $"User ({CreateUserModel.Username}) telah berhasil dibuat.";

            ToastService.ShowToast(intent, message);
            CreateUserModel.Reset();
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetProblemDetailsExtension();

            _errors.AddRange(extension.Errors);
        }

        IsLoading = false;
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
