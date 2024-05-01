using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class ApprovalStatusPieChart : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusReportModel>()
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

    protected string GetPointColor(ApprovalStatusReportModel model)
    {
        switch (model.ApprovalStatus)
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
