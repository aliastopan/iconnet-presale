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

    public void CloseTab(TabNavigation tab)
    {
        if (ActiveTabId == tab.Id)
        {
            return;
        }

        var tabToClose = _tabNavigations.Find(x => x.Id == tab.Id);
        _tabNavigations.Remove(tabToClose);

        Log.Warning("Closing tab: {0}", tab.Id);
    }
}
