namespace IConnet.Presale.WebApp.Components.Layout;

public partial class NavMenu
{
    [Inject] public SessionService SessionService { get; set; } = default!;

    public bool IsExpanded { get; set; } = true;
}
