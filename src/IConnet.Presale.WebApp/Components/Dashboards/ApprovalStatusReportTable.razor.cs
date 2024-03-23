using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ApprovalStatusReportTable : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];
}
