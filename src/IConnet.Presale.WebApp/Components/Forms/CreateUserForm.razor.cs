using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class CreateUserForm : ComponentBase
{
    private readonly List<Error> _errors = [];

    public CreateUserModel CreateUserModel { get; set; } = default!;
    public List<Error> Errors => _errors;
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

    protected async Task OnCreateUserAsync()
    {
        _errors.Clear();

        var isValid = CreateUserModel.TryValidate(out Error[] errors);

        if (!isValid)
        {
            _errors.AddRange(errors);
            LogSwitch.Debug("Errors {0}", _errors.Count);
        }


        LogSwitch.Debug("Username {0}", CreateUserModel.Username);
        LogSwitch.Debug("Password {0}", CreateUserModel.Password);
        LogSwitch.Debug("Confirm Password {0}", CreateUserModel.ConfirmPassword);
        LogSwitch.Debug("Job Shift {0}", CreateUserModel.JobShift);
        LogSwitch.Debug("Employment Status {0}", CreateUserModel.EmploymentStatus);
        LogSwitch.Debug("User Role {0}", CreateUserModel.UserRole);
        LogSwitch.Debug("Job Title {0}", CreateUserModel.JobTitle);

        await Task.CompletedTask;
    }
}
