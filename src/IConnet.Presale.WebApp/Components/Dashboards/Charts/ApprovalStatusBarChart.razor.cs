using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class ApprovalStatusBarChart : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusTransposeModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusTransposeModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusTransposeModel>
        {
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    Horizontal = false,
                    // BarHeight = "24px",
                    // ColumnWidth = "32px",
                    DataLabels = new PlotOptionsBarDataLabels
                    {
                        Total = new BarTotalDataLabels
                        {
                            Style = new BarDataLabelsStyle
                            {
                                FontWeight = "800",
                                FontSize = "16px",
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
            }
        };
    }

    protected string GetPointColor(ApprovalStatus approvalStatus)
    {
        switch (approvalStatus)
        {
            case ApprovalStatus.InProgress:
                return "#b2ce60";
            case ApprovalStatus.CloseLost:
                return "#02768f";
            case ApprovalStatus.Reject:
                return "#1c94ad";
            case ApprovalStatus.Expansion:
                return "#8fab3c";
            case ApprovalStatus.Approve:
                return "#0bd0d9";
            default:
                return "#ffffff";
        }
    }
}

    // Colors = ["#b2ce60", "#02768f", "#1c94ad", "#8fab3c", "#0bd0d9"]
    // Colors = ["#1e6bc9", "#202020", "#ff0033", "#ff7300", "#0e700e"]
    // Colors = ["#909295", "#231f20", "#02768f", "#1c94ad", "#0bd0d9"]