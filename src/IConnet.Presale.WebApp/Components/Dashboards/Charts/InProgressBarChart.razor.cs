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

    protected string GetInProgressDisplay(WorkPaperLevel workPaperLevel)
    {
        switch (workPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "Verifikasi";
            case WorkPaperLevel.Reinstated:
                return "Verifikasi (Reset)";
            case WorkPaperLevel.ImportVerified:
                return "Chat/Call Pick-Up";
            case WorkPaperLevel.Validating:
                return "Validasi";
            case WorkPaperLevel.WaitingApproval:
                return "Approval";
            default:
                throw new NotImplementedException("Invalid In-Progress Report Target");
        }
    }

    protected string GetPointColor(WorkPaperLevel workPaperLevel)
    {
        switch (workPaperLevel)
        {
            case WorkPaperLevel.ImportUnverified:
                return "#E12D4B";
            case WorkPaperLevel.Reinstated:
                return "#3bbdb0";
            case WorkPaperLevel.ImportVerified:
                return "#FFBF00";
            case WorkPaperLevel.Validating:
                return "#256FF5";
            case WorkPaperLevel.WaitingApproval:
                return "#B0EA57";
            default:
                return "#ffffff";
        }
    }
}