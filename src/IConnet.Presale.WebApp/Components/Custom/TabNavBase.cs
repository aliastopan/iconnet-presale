namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter]
    public TabNavigation Tab { get; set; }

    [Parameter]
    public EventCallback<TabNavigation> OnTabClick { get; set; }

    protected async Task ClickTabAsync()
    {
        await OnTabClick.InvokeAsync(Tab);
    }
}
