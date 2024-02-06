namespace IConnet.Presale.WebApp.Managers;

public class TabNavigationManager
{
    private readonly NavigationManager _navigationManager;
    private readonly List<TabNavigation> _tabNavigations = [];

    public TabNavigationManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public List<TabNavigation> TabNavigations => _tabNavigations;
    public string? ActiveTabId { get; set; }

    public void SelectTab(TabNavigation tabNavigation)
    {
        var notListed = !_tabNavigations.Any(x => x.Id == tabNavigation.Id);
        if (notListed)
        {
            _tabNavigations.Add(tabNavigation);
        }

        ActiveTabId = tabNavigation.Id;
        Log.Warning("Selected tab: {0}", ActiveTabId);
    }

    public void ChangeTab(TabNavigation tab)
    {
        if (ActiveTabId == tab.Id)
        {
            return;
        }

        _navigationManager.NavigateTo(tab.PageUrl);
        ActiveTabId = tab.Id;
        Log.Warning("Change tab: {0}", ActiveTabId);
    }

    // public void ChangeTab(FluentTab tab)
    // {
    //     Log.Warning("Change tab to: {0}", tab.Id);

    //     var tabNavigation = _tabNavigations.Find(x => x.Id == tab.Id);
    //     _navigationManager.NavigateTo(tabNavigation.PageUrl);
    // }

    // public void CloseTab(FluentTab tab)
    // {
    //     var tabActive = _tabNavigations.Find(x => x.Id == ActiveTabId);
    //     var tabToClose = _tabNavigations.Find(x => x.Id == tab.Id);
    //     int indexTabToClose = _tabNavigations.IndexOf(tabToClose);
    //     int indexTabActive = _tabNavigations.IndexOf(tabActive);

    //     if (indexTabToClose == indexTabActive)
    //     {
    //         ActiveTabId = tabActive.Id;
    //         // _navigationManager.NavigateTo(tabActive.PageUrl);
    //         Log.Warning("Active tab persist");
    //     }
    //     else
    //     {
    //         _navigationManager.NavigateTo(tabToClose.PageUrl);
    //         ActiveTabId = tabToClose.Id;
    //     }

    //     Log.Warning("Active tab: {0}, Close tab {1}", indexTabActive, indexTabToClose);

    //     return;
    // }
}
