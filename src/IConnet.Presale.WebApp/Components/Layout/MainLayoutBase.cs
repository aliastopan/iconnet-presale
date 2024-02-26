using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using IConnet.Presale.Shared.Validations;
using IConnet.Presale.WebApp.Components.Custom;
using System.Diagnostics;

namespace IConnet.Presale.WebApp.Components.Layout;

public class MainLayoutBase : LayoutComponentBase
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public ProtectedLocalStorage LocalStorage { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    public CustomErrorBoundary? ErrorBoundary { get; set; }

    protected override void OnInitialized()
    {
        if (ErrorBoundary?.CurrentException is not null)
        {
            LogSwitch.Debug("Recovering from {exception}", ErrorBoundary.CurrentException.GetType().Name);
            ErrorBoundary?.Recover();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationStateProvider.GetAuthenticationStateAsync();
    }

    protected void RedirectUnauthorized()
    {
        var homePage = new Regex(RegexPattern.HomePageUrl);
        var currentUrl = NavigationManager.Uri;

        if (!homePage.IsMatch(currentUrl))
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
