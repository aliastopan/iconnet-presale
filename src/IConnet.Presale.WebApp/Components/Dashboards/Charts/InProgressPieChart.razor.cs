using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Charts;

public partial class InProgressPieChart : ComponentBase
{
    [Parameter]
    public List<InProgressReportModel> Models { get; set; } = [];

    protected ApexChartOptions<InProgressReportModel> Options { get; set; } = default!;

    protected string GetPointColor(InProgressReportModel model)
    {
        switch (model.WorkPaperLevel)
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
