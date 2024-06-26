using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Components.Pages;

public class UserManagerPageBase : ComponentBase, IPageNavigation
{
    [Inject] protected TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] protected UserManager UserManager { get; set; } = default!;

    public bool IsInitialized { get; set; } = true;

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("user-manager", PageNavName.UserManager, PageRoute.UserManager);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);
    }
}
