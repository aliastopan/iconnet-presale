namespace IConnet.Presale.WebApp.Components;

public partial class Routes : ComponentBase
{
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] public BroadcastService BroadcastService { get; set; } = default!;

    public bool IsPreRender { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsPreRender = !(HttpContextAccessor.HttpContext is not null
            && HttpContextAccessor.HttpContext.Response.HasStarted);

        await BroadcastService.StartConnectionAsync();
    }
}
