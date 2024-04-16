using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class ApprovalStatusPieChart
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusReportModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusReportModel>();
    }

    protected string GetPointColor(ApprovalStatusReportModel model)
    {
        string x = EnumProcessor.EnumToDisplayString(ApprovalStatus.Approve);

        switch (model.ApprovalStatus)
        {
            case ApprovalStatus.InProgress:
                return "#909295";
            case ApprovalStatus.CloseLost:
                return "#231f20";
            case ApprovalStatus.Reject:
                return "#02768f";
            case ApprovalStatus.Expansion:
                return "#1c94ad";
            case ApprovalStatus.Approve:
                return "#0bd0d9";
            default:
                return "#ffffff";
        }
    }
}
