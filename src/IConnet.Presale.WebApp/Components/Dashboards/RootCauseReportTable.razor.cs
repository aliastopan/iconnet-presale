using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class RootCauseReportTable : ComponentBase
{
    [Parameter]
    public List<RootCauseReportModel> Models { get; set; } = [];
}
