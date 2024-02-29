using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Forms;

public partial class CreateUserForm : ComponentBase
{
    public CreateUserModel CreateUserModel { get; set; } = default!;

    protected Func<string, bool> OptionDisableSuperUser => option => option == OptionSelect.Role.SuperUser;

    protected override void OnInitialized()
    {
        CreateUserModel = new CreateUserModel();

        base.OnInitialized();
    }
}
