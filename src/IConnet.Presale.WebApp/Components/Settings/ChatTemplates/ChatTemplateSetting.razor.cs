namespace IConnet.Presale.WebApp.Components.Settings.ChatTemplates;

public partial class ChatTemplateSetting
{
    [Parameter]
    public IQueryable<string>? ModelAvailable { get; set; }

    protected string GridTemplateCols => GetGridTemplateCols();

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private string GetGridTemplateCols()
    {
        return $"{150}px";
    }
}
