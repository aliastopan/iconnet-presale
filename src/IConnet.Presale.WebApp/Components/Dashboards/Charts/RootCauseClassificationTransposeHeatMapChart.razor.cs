using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class RootCauseClassificationTransposeHeatMapChart : ComponentBase
{
    [Parameter]
    public List<RootCauseClassificationReportTransposeModel> Models { get; set; } = [];

    protected ApexChartOptions<RootCauseClassificationReportTransposeModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<RootCauseClassificationReportTransposeModel>()
        {
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false },
            },
            Colors = new List<string> { "#E12D4B" }
        };
    }

    protected int GetCharHeight()
    {
        int minHeight = 128;
        int height = 32;
        int totalRootCause = Models.SelectMany(x => x.ClassificationMetrics.Keys).Distinct().Count();

        int chartHeight = height * totalRootCause;

        return chartHeight < minHeight
            ? minHeight
            : chartHeight;
    }
}
