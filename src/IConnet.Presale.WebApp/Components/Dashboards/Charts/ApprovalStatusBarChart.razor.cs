using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class ApprovalStatusBarChart : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportTransposeModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusReportTransposeModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusReportTransposeModel>
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
                return "#256FF5";
            case ApprovalStatus.CloseLost:
                return "#454545";
            case ApprovalStatus.Reject:
                return "#E12D4B";
            case ApprovalStatus.Expansion:
                return "#FFBF00";
            case ApprovalStatus.Approve:
                return "#B0EA57";
            default:
                return "#ffffff";
        }
    }
}
