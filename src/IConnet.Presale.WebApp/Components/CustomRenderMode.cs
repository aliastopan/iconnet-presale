using Microsoft.AspNetCore.Components.Web;

namespace IConnet.Presale.WebApp.Components;

public static class CustomRenderMode
{
    public static InteractiveServerRenderMode DisablePreRender(this InteractiveServerRenderMode _)
    {
        return new InteractiveServerRenderMode(prerender: false);
    }
}
