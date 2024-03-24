using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ChatCallMulaiAgingReportTable : ComponentBase
{
    [Parameter]
    public List<ChatCallMulaiAgingReportModel> Models { get; set; } = [];
}
