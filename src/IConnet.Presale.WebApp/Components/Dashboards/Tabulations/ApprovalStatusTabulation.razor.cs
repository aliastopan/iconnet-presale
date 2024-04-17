using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ApprovalStatusTabulation : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    protected string GetChartWidthStyle()
    {
        int width = 125;
        int totalColumn = Models.Select(x => x.ApprovalStatus).Distinct().Count();

        return $"width: {(width * totalColumn) - 35}px;";
    }
}
