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

    public void ChangeTab(FluentTab tab)
    {
        Log.Warning("Change tab to: {0}", tab.Id);

        var tabNavigation = _tabNavigations.Find(x => x.Id == tab.Id);
        _navigationManager.NavigateTo(tabNavigation.PageUrl);
    }
}
