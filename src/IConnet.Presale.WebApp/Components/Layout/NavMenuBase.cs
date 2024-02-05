namespace IConnet.Presale.WebApp.Components.Layout;

public class NavMenuBase : LayoutComponentBase
{
    public bool IsExpanded { get; set; } = true;

    public void OnClick()
    {
        Log.Warning("OnClick");
    }
}
