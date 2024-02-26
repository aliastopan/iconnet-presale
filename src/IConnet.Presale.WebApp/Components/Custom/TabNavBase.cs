namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter] public TabNavigationModel TabNavigation { get; set; }
    [Parameter] public EventCallback<TabNavigationModel> OnTabChange { get; set; }
    [Parameter] public EventCallback<TabNavigationModel> OnTabClose { get; set; }
    [Parameter] public string ActiveTabId { get; set; } = default!;
    public bool IsActive => TabNavigation.Id == ActiveTabId;

    protected async Task ChangeTabAsync()
    {
        await OnTabChange.InvokeAsync(TabNavigation);
    }

    protected async Task CloseTabAsync()
    {
        await OnTabClose.InvokeAsync(TabNavigation);
    }
}
