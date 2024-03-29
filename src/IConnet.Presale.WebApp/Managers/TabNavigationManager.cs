namespace IConnet.Presale.WebApp.Managers;

public class TabNavigationManager
{
    private readonly NavigationManager _navigationManager;
    private readonly List<TabNavigationModel> _tabNavigations = [];
    private readonly Stack<TabNavigationModel> _visitedTabs = [];
    private string? _activeTabId;
    public Action _stateHasChanged = default!;

    public TabNavigationManager(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public List<TabNavigationModel> TabNavigations => _tabNavigations;
    public string? ActiveTabId => _activeTabId;

    public void StateHasChanged(Action stateHasChanged)
    {
        _stateHasChanged = stateHasChanged;
    }

    public void SelectTab(IPageNavigation pageNavigation)
    {
        var tabToSelect = pageNavigation.PageDeclaration();
        var notListed = !_tabNavigations.Any(x => x.Id == tabToSelect.Id);

        if (notListed)
        {
            _tabNavigations.Add(tabToSelect);
        }

        _activeTabId = tabToSelect.Id;
        _visitedTabs.Push(tabToSelect);

        _stateHasChanged();
    }

    public void ChangeTab(TabNavigationModel tabToChange)
    {
        if (_activeTabId == tabToChange.Id)
        {
            return;
        }

        _navigationManager.NavigateTo(tabToChange.PageUrl);
        _activeTabId = tabToChange.Id;
    }

    public void CloseTab(TabNavigationModel tabToClose)
    {
        if (_tabNavigations.Count <=  1 && _activeTabId == tabToClose.Id)
        {
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
    }

    public void NavigateBack()
    {
        if (_visitedTabs.Count <= 1)
        {
            return;
        }

        _visitedTabs.Pop();
        var previousTab = _visitedTabs.Peek();

        _navigationManager.NavigateTo(previousTab.PageUrl);
        _visitedTabs.Pop(); // pop unintentionally stack.push from `SelectTab` after `NavigateTo`
        _activeTabId = previousTab.Id;
    }
}
