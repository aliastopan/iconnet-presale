using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Pages;

public class UserManagerPageBase : ComponentBase, IPageNavigation
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;

    private List<UserAccountModel> _userAccounts { get; set; } = [];

    protected IQueryable<UserAccountModel>? UserAccounts => _userAccounts
        .Where(user => user.UserRole < UserRole.SuperUser)
        .OrderBy(user => user.Username)
        .AsQueryable();

    public bool IsInitialized { get; set; } = true;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("user-manager", PageNavName.UserManager, PageRoute.UserManager);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);
    }

    protected override async Task OnInitializedAsync()
    {
        IsInitialized = false;

        _userAccounts = await UserManager.GetUserAccountsAsync();

        IsInitialized = true;
    }

    protected async Task ReloadUserAccountAsync()
    {
        IsInitialized = false;
        this.StateHasChanged();

        _userAccounts.Clear();

        _userAccounts = await UserManager.GetUserAccountsAsync();
        await UserManager.SetPresaleOperatorsAsync();

        IsInitialized = true;
        this.StateHasChanged();

        LogSwitch.Debug("Reload User Accounts");
    }
}
