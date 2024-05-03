using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class InProgressPieChart : ComponentBase
{
    [Parameter]
    public List<InProgressReportModel> Models { get; set; } = [];

    protected ApexChartOptions<InProgressReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<InProgressReportModel>()
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
            }
        };
    }

    protected string GetPointColor(InProgressReportModel model)
    {
        switch (model.WorkPaperLevel)
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
