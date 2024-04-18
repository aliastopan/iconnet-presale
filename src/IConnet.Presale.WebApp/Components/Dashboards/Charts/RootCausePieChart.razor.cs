using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class RootCausePieChart : ComponentBase
{
    [Parameter]
    public List<RootCauseReportModel> Models { get; set; } = [];

    protected ApexChartOptions<RootCauseReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<RootCauseReportModel>()
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
            Colors = ["#024756", "#02768f", "#1c94ad", "#0cd0d9", "#5e7028", "#8fab3c", "#b2ce60", "#ccff34"]
            // Colors = ["#02768f", "#197a91", "#307e93", "#479295", "#5ea697", "#75ba99", "#8cce9b", "#a3e29d", "#baf69f", "#d1c15a", "#e78c15", "#fd8181"]
            // Colors = ["#02768f", "#107586", "#1e747d", "#2c7374", "#3a726b", "#487162", "#567059", "#646f50", "#727e47", "#808d3e", "#8e9c35", "#9cab2c", "#b2ce60"]
            // Colors = ["#02768f", "#068497", "#0a9aa0", "#0eb0a9", "#12c6b2", "#16dcbb", "#1af2c4", "#1effcd", "#61ebd3", "#a3d7d9", "#e6c3de", "#d0bb88", "#b2ce60"]
            // Colors = ["#02768F", "#0BD0D9", "#CFC665", "#B2CE60"]
        };
    }
}
