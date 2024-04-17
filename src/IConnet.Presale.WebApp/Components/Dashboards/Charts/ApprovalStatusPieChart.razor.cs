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
        string x = EnumProcessor.EnumToDisplayString(ApprovalStatus.Approve);

        switch (model.ApprovalStatus)
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
