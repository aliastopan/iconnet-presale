using Microsoft.AspNetCore.Components;

namespace IConnet.Presale.WebApp.Templates;

public static class HtmlFragment
{
    public static RenderFragment EmptyContent => builder =>
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "class", "empty-table");
        builder.AddAttribute(2, "style", "height: 305px; background-color: crimson;");
        builder.AddContent(3, "");
        builder.CloseElement();
    };

    public static RenderFragment MeterUnitOfMeasurement => builder =>
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "class", "meter-unit");
        builder.AddContent(2, "meter");
        builder.CloseElement();
    };
}
