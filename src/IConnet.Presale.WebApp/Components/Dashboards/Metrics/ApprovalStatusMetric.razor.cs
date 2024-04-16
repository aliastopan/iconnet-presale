using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Metrics;

public partial class ApprovalStatusMetric : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    [Parameter]
    public List<ApprovalStatusMetricModel> Metrics { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusMetricModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusMetricModel>
        {
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    Horizontal = true
                }
            },
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false },
                Stacked = true
            },
            Xaxis = new XAxis
            {
                AxisTicks = new AxisTicks
                {
                    Height = 10,
                    Show = false
                }
            },
            Colors = ["#1e6bc9", "#202020", "#ff0033", "#ff7300", "#0e700e"]
        };
    }
}
