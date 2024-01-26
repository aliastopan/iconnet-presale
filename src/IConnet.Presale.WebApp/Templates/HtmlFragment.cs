using Microsoft.AspNetCore.Components;

namespace IConnet.Presale.WebApp.Templates;

public static class HtmlFragment
{
    public static RenderFragment EmptyContent => builder =>
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "class", "p-2");
        builder.AddContent(2, "Data tidak tersedia.");
        builder.CloseElement();
    };
}
