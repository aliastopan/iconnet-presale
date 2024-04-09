namespace IConnet.Presale.WebApp.Components.Settings;

public partial class RootCauseSetting
{
    [Parameter]
    public IQueryable<RootCauseSettingModel>? Models { get; set; }

    protected string GridTemplateCols => GetGridTemplateCols();

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }

    private string GetGridTemplateCols()
    {
        return $"{350}px {80}px {80}px {80}px;";
    }
}
