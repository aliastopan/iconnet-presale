using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class RootCauseTabulation : ComponentBase
{
    [Parameter]
    public List<RootCauseReportModel> Models { get; set; } = [];
}
