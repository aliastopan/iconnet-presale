namespace IConnet.Presale.WebApp.Components.Pages;

public class UserManagerPageBase : ComponentBase, IPageNavigation
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;

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

        await Task.CompletedTask;

        IsInitialized = true;
    }

}
