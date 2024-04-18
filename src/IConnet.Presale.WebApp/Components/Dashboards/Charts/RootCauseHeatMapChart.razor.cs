using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class RootCauseHeatMapChart : ComponentBase
{
    [Parameter]
    public List<RootCauseReportModel> Models { get; set; } = [];

    protected ApexChartOptions<RootCauseReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<RootCauseReportModel>()
        {
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false },
            },
            Colors = new List<string> { "#02768f" }
        };
    }

    protected int GetCharHeight()
    {
        int minHeight = 300;
        int height = 65;
        int totalColumn = Models.SelectMany(x => x.RootCausePerOffice.Keys).Distinct().Count();

        int chartWidth = height * totalColumn;

        return chartWidth < minHeight
            ? minHeight
            : chartWidth;
    }

    protected int GetChartWidth()
    {
        int minWidth = 500;
        int width = 50;
        int totalColumn = Models.First().RootCausePerOffice.Keys.ToList().Count;

        int chartWidth = width * totalColumn;

        return chartWidth < minWidth
            ? minWidth
            : chartWidth;
    }
}
