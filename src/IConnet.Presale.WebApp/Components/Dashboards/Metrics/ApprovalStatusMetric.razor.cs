using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Metrics;

public partial class ApprovalStatusMetric : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusMetricModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusMetricModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusMetricModel>
        {
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    Horizontal = false,
                    DataLabels = new PlotOptionsBarDataLabels
                    {
                        Total = new BarTotalDataLabels
                        {
                            Style = new BarDataLabelsStyle
                            {
                                FontWeight = "800",
                                Color = "#231f20"
                            }
                        }
                    }
                }
            },
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false },
                Stacked = false
            },
            Xaxis = new XAxis
            {
                AxisTicks = new AxisTicks
                {
                    Height = 10,
                    Show = false
                }
            },
            // Colors = ["#1e6bc9", "#202020", "#ff0033", "#ff7300", "#0e700e"]
            Colors = ["#909295", "#231f20", "#02768f", "#1c94ad", "#0bd0d9"]

        };
    }
}
