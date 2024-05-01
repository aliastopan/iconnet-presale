using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class InProgressBarChart : ComponentBase
{
    [Parameter]
    public List<InProgressReportTransposeModel> Models { get; set; } = [];

    protected ApexChartOptions<InProgressReportTransposeModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<InProgressReportTransposeModel>
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

    protected string GetPointColor(WorkPaperLevel workPaperLevel)
    {
        switch (workPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "#024756";
            case WorkPaperLevel.Reinstated:
                return "#02768f";
            case WorkPaperLevel.ImportVerified:
                return "#1c94ad";
            case WorkPaperLevel.Validating:
                return "#0bd0d9";
            case WorkPaperLevel.WaitingApproval:
                return "#79f8ff";
            default:
                return "#ffffff";
        }
    }
}
