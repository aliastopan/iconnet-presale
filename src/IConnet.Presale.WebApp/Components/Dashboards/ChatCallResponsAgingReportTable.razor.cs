using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ChatCallResponsAgingReportTable
{
    [Parameter]
    public List<ChatCallResponsAgingReportModel> Models { get; set; } = [];
}
