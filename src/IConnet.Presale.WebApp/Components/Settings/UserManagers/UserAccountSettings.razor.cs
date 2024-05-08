using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Settings.UserManagers;

public partial class UserAccountSettings : ComponentBase
{
    [Inject] protected UserManager UserManager { get; set; } = default!;

    [Parameter]
    public List<UserAccountModel> UserAccounts { get; set; } = [];

    protected override void OnInitialized()
    {
        Log.Information("Credentials {0}", UserAccounts.Count);
    }
}
