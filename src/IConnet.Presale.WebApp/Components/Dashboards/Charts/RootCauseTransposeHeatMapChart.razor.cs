using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class RootCauseTransposeHeatMapChart : ComponentBase
{
    [Parameter]
    public List<RootCauseReportTransposeModel> Models { get; set; } = [];

    protected ApexChartOptions<RootCauseReportTransposeModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<RootCauseReportTransposeModel>()
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
        int height = 32;
        int availableRootCause = Models.SelectMany(x => x.RootCauseMetrics.Keys).Distinct().Count();

        return height * availableRootCause;
    }
}
