using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ApprovalSlaTabulation
{
    [Parameter]
    public List<ApprovalAgingReportModel> Models { get; set; } = [];

    [Parameter]
    public bool IsPageView { get; set; }
}
