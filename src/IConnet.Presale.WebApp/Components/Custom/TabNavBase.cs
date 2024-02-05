namespace IConnet.Presale.WebApp.Components.Custom;

public class TabNavBase : ComponentBase
{
    [Parameter]
    public TabNavigation Tab { get; set; }

    protected void CloseTab()
    {
        Log.Warning("Closing {0}", Tab.Id);
    }
}
