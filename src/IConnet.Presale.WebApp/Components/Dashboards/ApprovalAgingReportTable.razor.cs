using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ApprovalAgingReportTable
{
    [Parameter]
    public List<ApprovalAgingReportModel> Models { get; set; } = [];
}
