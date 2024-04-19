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
                return "#25a9f5";
            case ApprovalStatus.CloseLost:
                return "#474747";
            case ApprovalStatus.Reject:
                return "#fc4848";
            case ApprovalStatus.Expansion:
                return "#f5f257";
            case ApprovalStatus.Approve:
                return "#8fe257";
            default:
                return "#ffffff";
        }
    }
}
