using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class RootCauseClassificationPieChart
{
    [Parameter]
    public List<RootCauseClassificationReportModel> Models { get; set; } = [];

    protected ApexChartOptions<RootCauseClassificationReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<RootCauseClassificationReportModel>()
        {
            DataLabels = new DataLabels
            {
                Enabled = true,
                Style = new DataLabelsStyle
                {
                    FontSize = "16px",
                    Colors = ["#202020"]
                },
                DropShadow = new DropShadow
                {
                    Enabled = false
                }
            },
            Colors =
            [
                "#E12D4B",
                "#FF7F50",
                "#FFBF00",
                "#DFFF00",
                "#B0EA57",
                "#9FE2BF",
                "#40E0D0",
                "#6495ED",
                "#256FF5",
                "#CCCCFF",
            ]
        };
    }
}
