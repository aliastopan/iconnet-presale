using IConnet.Presale.WebApp.Components.Layout;

namespace IConnet.Presale.WebApp.Managers;

public class TabNavigationManager
{
    private readonly NavigationManager _navigationManager;
    private readonly List<TabNavigation> _tabNavigations = [];

    public TabNavigationManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _tabNavigations.Add(NavMenuBase.HomePage());
        ActiveTabId = _tabNavigations.First().Id;

    }

    public List<TabNavigation> TabNavigations => _tabNavigations;
    public string ActiveTabId { get; set; }


    public void SelectTab(TabNavigation tabNavigation)
    {
        var notListed = !_tabNavigations.Any(x => x.Id == tabNavigation.Id);
        if (notListed)
        {
            _tabNavigations.Add(tabNavigation);
        }

        // _navigationManager.NavigateTo(tabNavigation.PageUrl);
        ActiveTabId = tabNavigation.Id;
    }
}
