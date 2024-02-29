using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class CreateUserForm : ComponentBase
{
    [Inject] public IIdentityHttpClient IdentityHttpClient { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private readonly List<Error> _errors = [];

    public CreateUserModel CreateUserModel { get; set; } = default!;
    public List<Error> Errors => _errors;
    public bool IsLoading { get; set; } = false;
    public bool ShowErrorMessages => _errors.Count > 0;

    protected Func<string, bool> OptionDisableSuperUser => option => option == OptionSelect.Role.SuperUser;

    protected override void OnInitialized()
    {
        CreateUserModel = new CreateUserModel
        {
            JobShift = OptionSelect.Shift.ShiftOptions.First(),
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
            LogSwitch.Debug("Errors {0}", _errors.Count);
            _errors.AddRange(errors);

            return;
        }

        await CreateUserAsync();
    }

    public async Task CreateUserAsync()
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
            CreateUserModel.JobShift,
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
}
