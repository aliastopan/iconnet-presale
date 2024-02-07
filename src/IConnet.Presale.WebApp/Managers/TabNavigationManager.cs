namespace IConnet.Presale.WebApp.Managers;

public class TabNavigationManager
{
    private readonly NavigationManager _navigationManager;
    private readonly List<TabNavigation> _tabNavigations = [];
    private string? _activeTabId;

    public TabNavigationManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public List<TabNavigation> TabNavigations => _tabNavigations;

    public void SelectTab(TabNavigation tabToSelect)
    {
        var notListed = !_tabNavigations.Any(x => x.Id == tabToSelect.Id);
        if (notListed)
        {
            _tabNavigations.Add(tabToSelect);
        }

        _activeTabId = tabToSelect.Id;
        Log.Warning("Selected tab: {0}", _activeTabId);
    }

    public void ChangeTab(TabNavigation tabToChange)
    {
        if (_activeTabId == tabToChange.Id)
        {
            return;
        }

        _navigationManager.NavigateTo(tabToChange.PageUrl);
        _activeTabId = tabToChange.Id;
        Log.Warning("Change tab: {0}", _activeTabId);
    }

    public void CloseTab(TabNavigation tabToClose)
    {
        if (_tabNavigations.Count <=  1 && _activeTabId == tabToClose.Id)
        {
            Log.Warning("Cannot remove the last tab");
            return;
        }

        int indexTabToClose = _tabNavigations.FindIndex(x => x.Id == tabToClose.Id);

        if (_activeTabId == tabToClose.Id)
        {
            var tabToShift = indexTabToClose > 0
                ? _tabNavigations[indexTabToClose - 1]
                : _tabNavigations[indexTabToClose + 1];

            _navigationManager.NavigateTo(tabToShift.PageUrl);
            _activeTabId = tabToShift.Id;
        }

        _tabNavigations.Remove(tabToClose);
        Log.Warning("Closing tab: {0}", tabToClose.Id);
    }
}
